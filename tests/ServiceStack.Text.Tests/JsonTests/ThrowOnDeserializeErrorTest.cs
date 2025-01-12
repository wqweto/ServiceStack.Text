﻿using System.Runtime.Serialization;
using NUnit.Framework;

namespace ServiceStack.Text.Tests.JsonTests
{
	[TestFixture]
	public class ThrowOnDeserializeErrorTest
	{
        [Test]
        [ExpectedException(typeof(SerializationException), ExpectedMessage = "Failed to set property 'idBadProt' with 'abc'")]
        public void Throws_on_protected_setter()
        {
            JsConfig.Reset();
            JsConfig.ThrowOnDeserializationError = true;

            string json = @"{""idBadProt"":""abc"", ""idGood"":""2"" }";
            JsonSerializer.DeserializeFromString(json, typeof(TestDto));
        }

		[Test]
        [ExpectedException(typeof(SerializationException), ExpectedMessage = "Failed to set property 'idBad' with 'abc'")]
		public void Throws_on_incorrect_type()
		{
			JsConfig.Reset();
			JsConfig.ThrowOnDeserializationError = true;

			string json = @"{""idBad"":""abc"", ""idGood"":""2"" }";
    		JsonSerializer.DeserializeFromString(json, typeof(TestDto));
		}

		[Test]
		public void Throws_on_incorrect_type_with_data_set()
		{
			JsConfig.Reset();
			JsConfig.ThrowOnDeserializationError = true;

            try {
			    string json = @"{""idBad"":""abc"", ""idGood"":""2"" }";
    		    JsonSerializer.DeserializeFromString(json, typeof(TestDto));
                Assert.Fail("Exception should have been thrown.");
            } catch (SerializationException ex) {
                Assert.That(ex.Data, Is.Not.Null);
                Assert.That(ex.Data["propertyName"], Is.EqualTo("idBad"));
                Assert.That(ex.Data["propertyValueString"], Is.EqualTo("abc"));
                Assert.That(ex.Data["propertyType"], Is.EqualTo(typeof(int)));
            }
		}

		[Test]
		public void TestDoesNotThrow()
		{
			JsConfig.Reset();
			JsConfig.ThrowOnDeserializationError = false;
			string json = @"{""idBad"":""abc"", ""idGood"":""2"" }";
			JsonSerializer.DeserializeFromString(json, typeof(TestDto));
		}

		[Test]
		public void TestReset()
		{
			JsConfig.Reset();
			Assert.IsFalse(JsConfig.ThrowOnDeserializationError);
			JsConfig.ThrowOnDeserializationError = true;
			Assert.IsTrue(JsConfig.ThrowOnDeserializationError);
			JsConfig.Reset();
			Assert.IsFalse(JsConfig.ThrowOnDeserializationError);
		}

		[DataContract]
		class TestDto
		{
            [DataMember(Name = "idBadProt")]
            public int protId { get; protected set; }
            [DataMember(Name = "idGood")]
			public int IdGood { get; set; }
			[DataMember(Name = "idBad")]
			public int IdBad { get; set; }
        }
	}
}

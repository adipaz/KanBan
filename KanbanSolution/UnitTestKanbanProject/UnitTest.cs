using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KanbanProject.BusinessLayer;
using KanbanProject.InterfaceLayer;


namespace UnitTestKanbanProject
{
    [TestClass]
    public class UnitTest
    {

        //checking if password is valid
        [TestMethod]
        public void Test_TooShortPassword()
        {
            bool answer = User.IsPasswordlValid("Ab3");
            Assert.IsFalse(answer);

        }

        [TestMethod]
        public void Test_TooLongPassword()
        {
            bool answer = User.IsPasswordlValid("Abskasdjasd1819101PPOANSNq");
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_HasUpperLetter()
        {
            bool answer = User.IsPasswordlValid("abcd345");
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_HasLowerLetter()
        {
            bool answer = User.IsPasswordlValid("ABD123");
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_HasNumber()
        {
            bool answer = User.IsPasswordlValid("abcdABCD");
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_ValidPassword()
        {
            bool answer = User.IsPasswordlValid("abcdEF345");
            Assert.IsTrue(answer);
        }

        //checking if title is valid

        [TestMethod]
        public void Test_TitleTooShort()
        {
            bool answer = Task.IsValidTitle("");
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_TitleTooLong()
        {
            bool answer = Task.IsValidTitle("sakjdbASBDjdbJDBaldnADasmnnndkaslKSLADLSadkskdADNandaMDAMd");
            Assert.IsFalse(answer);
        }


        [TestMethod]
        public void Test_NullTitle()
        {
            bool answer = Task.IsValidTitle(null);
            Assert.IsFalse(answer);
        }

        [TestMethod]
        public void Test_ValidTitle()
        {
            bool answer = Task.IsValidTitle("Great title");
            Assert.IsTrue(answer);
        }
    }
}


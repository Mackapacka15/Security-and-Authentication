using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityAndAuth.Utils;

namespace SecurityAndAuth.Tests
{
    [TestClass]
    public class PasswordValidatorTests
    {
        private readonly IPasswordValidator _validator = new PasswordValidator();

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_ForCommonPasswords()
        {
            Assert.IsFalse(_validator.IsValidPassword("password"));
            Assert.IsFalse(_validator.IsValidPassword("123456"));
            Assert.IsFalse(_validator.IsValidPassword("qwerty"));
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_ForShortPasswords()
        {
            Assert.IsFalse(_validator.IsValidPassword("short"));
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnTrue_ForStrongPasswords()
        {
            Assert.IsTrue(_validator.IsValidPassword("StrongPass123!"));
        }

        [TestMethod]
        public void PreventXss_ShouldEncodeHtmlTags()
        {
            Assert.AreEqual(
                "&lt;script&gt;alert('XSS');&lt;/script&gt;",
                _validator.PreventXss("<script>alert('XSS');</script>")
            );

            Assert.AreEqual("&lt;b&gt;Bold&lt;/b&gt;", _validator.PreventXss("<b>Bold</b>"));
        }

        [TestMethod]
        public void SqlEscape_ShouldEscapeSingleQuotes()
        {
            Assert.AreEqual("O''Reilly", PasswordValidator.SqlEscape("O'Reilly"));
        }

        [TestMethod]
        public void SqlEscape_ShouldNotEscapeNonSingleQuotes()
        {
            Assert.AreEqual("Hello", PasswordValidator.SqlEscape("Hello"));
        }

        [TestMethod]
        public void SqlEscape_ShouldEscapeMultipleSingleQuotes()
        {
            Assert.AreEqual("O''''Reilly", PasswordValidator.SqlEscape("O''Reilly"));
        }
    }
}

namespace Fanex.NAP.Core.Web.Auth
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Authorize information.
    /// </summary>
    public class AuthInfo
    {
        private int _userId;

        private int _onuserId;

        public int UserId
        {
            get
            {
                return this._userId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "userid");
                }

                this._userId = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return this._onuserId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "onUserid");
                }

                this._onuserId = value;
            }
        }

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
            AuthInfo authInfo = (AuthInfo)serializer.Deserialize(new StringReader(authInfoXml));
            return authInfo;
        }

        /// <summary>
        /// Authorize action handler.
        /// </summary>
        /// <returns>XML string.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Reviewed")]
        public string ToXml()
        {
            string xml = string.Empty;
            using (var sw = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(AuthInfo));

                serializer.Serialize(sw, this);
                sw.Close();
                xml = sw.ToString();
            }

            return xml;
        }
    }
}
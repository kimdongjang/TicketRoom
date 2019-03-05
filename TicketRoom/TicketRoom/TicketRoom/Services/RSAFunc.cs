using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TicketRoom.Services
{
    public class RSAFunc
    {
        private static RSAFunc _instance;

        //  'protected' 로 생성자를 만듦
        public RSAFunc() { }
        //Static으로 메서드 생성
        public static RSAFunc Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
                _instance = new RSAFunc();

            // 다중 쓰레드 환경일 경우 Lock이 필요함
            //if (_instance == null)
            //{
            //    lock (_synLock)
            //    {
            //        _instance = new WBdb();
            //    }
            //}

            return _instance;
        }
        // DB에서 홈 인덱스로 홈페이지 가져오기

        #region RSA 암호화 방식 관련
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        RSAParameters privateKey;
        RSAParameters publicKey;
        public string privateKeyText = string.Empty;
        string publicKeyText = string.Empty;
        #endregion

        #region RSA 암호화 방식 관련
        //RSA 암호화 개인키 세팅 함수
        public void SetRSA(string key)
        {
            ///회원 가입시 ------ RSA 키값이 존재 하지 않을때
            ///개인키 생성
            if (key.Equals("Start"))
            {
                privateKey = RSA.Create().ExportParameters(true);
                rsa.ImportParameters(privateKey);
                privateKeyText = rsa.ToXmlString(true);

                publicKey = new RSAParameters();
                publicKey.Modulus = privateKey.Modulus;
                publicKey.Exponent = privateKey.Exponent;
                rsa.ImportParameters(publicKey);
                publicKeyText = rsa.ToXmlString(false);
            }
            else
            {
                // 개인키 생성
                string s = key;
                rsa.FromXmlString(s);
                privateKey = rsa.ExportParameters(true);
                rsa.ImportParameters(privateKey);
                privateKeyText = rsa.ToXmlString(true);

                publicKey = new RSAParameters();
                publicKey.Modulus = privateKey.Modulus;
                publicKey.Exponent = privateKey.Exponent;
                rsa.ImportParameters(publicKey);
                publicKeyText = rsa.ToXmlString(false);
            }
        }

        #region LDH 암호화 복호화 함수
        //==================================================LDH RSA 암호화
        public string RSAEncrypt(string getValue)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(); //암호화
            rsa.FromXmlString(publicKeyText);
            //암호화할 문자열을 UFT8인코딩
            byte[] inbuf = (new UTF8Encoding()).GetBytes(getValue);
            //암호화
            byte[] encbuf = rsa.Encrypt(inbuf, false);
            //암호화된 문자열 Base64인코딩
            return Convert.ToBase64String(encbuf);
        }
        //================================================================
        //==================================================LDH RSA 복호화
        public string RSADecrypt(string getValue)
        {
            //RSA객체생성
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(); //복호화
            rsa.FromXmlString(privateKeyText);
            //sValue문자열을 바이트배열로 변환
            byte[] srcbuf = Convert.FromBase64String(getValue);
            //바이트배열 복호화
            byte[] decbuf = rsa.Decrypt(srcbuf, false);
            //복호화 바이트배열을 문자열로 변환
            string sDec = (new UTF8Encoding()).GetString(decbuf, 0, decbuf.Length);
            return sDec;
        }
        //================================================================
        #endregion
        #endregion
    }
}

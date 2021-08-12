using System.Linq;

namespace Classifieds.Helpers
{
    /// <summary>
    /// This library is a very simple two-way hashing used to obfuscate open text when it might be exposed
    /// This does not make reverse engineering impossible, it simply increases the difficulty.
    /// If the hacker does not have access to this library, the reverse engineering is difficult and that
    /// is enough to slow down or stop most attackers. 
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// Simple text obfuscation in a way that allows difficult to hack, but simple to reverse encryption
        /// </summary>
        /// <param name="stringToProcess">The text string to obfuscate.</param>
        /// <returns>The obfuscated text</returns>
        public static string TextEncode(string stringToProcess)
        {
            var byteArray = System.Text.Encoding.ASCII.GetBytes(stringToProcess);

            byteArray = MixUp7(byteArray);
            byteArray = MixUp3(byteArray);
            byteArray = SwapChar(byteArray);
            byteArray = Reverse(byteArray);
            byteArray = Xor1(byteArray);

            var enc = new System.Text.ASCIIEncoding();
            stringToProcess = enc.GetString(byteArray);

            return stringToProcess;
        }

        /// <summary>
        /// This takes in previously obfuscated text and returns the original text. 
        /// </summary>
        /// <param name="stringToProcess"></param>
        /// <returns>The original string that was previously processed by TextEncode.</returns>
        public static string TextDecode(string stringToProcess)
        {
            var byteArray = System.Text.Encoding.ASCII.GetBytes(stringToProcess);

            byteArray = Xor1(byteArray);
            byteArray = Reverse(byteArray);
            byteArray = SwapChar(byteArray);
            byteArray = MixUp3(byteArray);
            byteArray = MixUp7(byteArray);

            var enc = new System.Text.ASCIIEncoding();
            stringToProcess = enc.GetString(byteArray);

            return stringToProcess;
        }

        private static byte[] Xor1(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }
            if (!byteArray.Any())
            {
                return byteArray;
            }

            for (var i = 0; i < byteArray.Count(); i++)
            {
                byteArray[i] ^= 1;
            }

            return byteArray;
        }

        /// <summary>
        /// Swaps bytes 1-4, 2-5, 3-6 and leaves 7 unchanged, repeats this for the length of the sting
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private static byte[] MixUp7(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }
            if (byteArray.Count() < 7)
            {
                return byteArray;
            }

            for (var i = 6; i < byteArray.Count(); i += 7)
            {
                var holdByte = byteArray[i - 1];
                byteArray[i - 1] = byteArray[i - 4];
                byteArray[i - 4] = holdByte;

                holdByte = byteArray[i - 2];
                byteArray[i - 2] = byteArray[i - 5];
                byteArray[i - 5] = holdByte;

                holdByte = byteArray[i - 3];
                byteArray[i - 3] = byteArray[i - 6];
                byteArray[i - 6] = holdByte;
            }

            return byteArray;
        }

        private static byte[] MixUp3(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }
            if (byteArray.Count() < 3)
            {
                return byteArray;
            }

            for (var i = 2; i <= byteArray.Count(); i += 3)
            {
                var holdByte = byteArray[i - 1];
                byteArray[i - 1] = byteArray[i - 2];
                byteArray[i - 2] = holdByte;
            }

            return byteArray;
        }

        private static byte[] Reverse(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }
            if (!byteArray.Any())
            {
                return byteArray;
            }

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            byteArray.Reverse();

            return byteArray;
        }

        private static byte[] SwapChar(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }
            if (!byteArray.Any())
            {
                return byteArray;
            }

            byte HoldByte;
            for (int i = 0; i < byteArray.Count(); i++)
            {
                HoldByte = byteArray[i];

                if (HoldByte == 89) byteArray[i] = 44;
                if (HoldByte == 44) byteArray[i] = 89;
                
                if (HoldByte == 90) byteArray[i] = 59;
                if (HoldByte == 59) byteArray[i] = 90;

                if (HoldByte == 91) byteArray[i] = 37;
                if (HoldByte == 37) byteArray[i] = 91;

                if (HoldByte == 92) byteArray[i] = 62;
                if (HoldByte == 62) byteArray[i] = 92;

                if (HoldByte == 93) byteArray[i] = 40;
                if (HoldByte == 40) byteArray[i] = 93;

                if (HoldByte == 94) byteArray[i] = 53;
                if (HoldByte == 53) byteArray[i] = 94;

                if (HoldByte == 95) byteArray[i] = 58;
                if (HoldByte == 58) byteArray[i] = 95;

                if (HoldByte == 96) byteArray[i] = 49;
                if (HoldByte == 49) byteArray[i] = 96;

                if (HoldByte == 97) byteArray[i] = 66;
                if (HoldByte == 66) byteArray[i] = 97;

                if (HoldByte == 98) byteArray[i] = 56;
                if (HoldByte == 56) byteArray[i] = 98;

                if (HoldByte == 99) byteArray[i] = 33;
                if (HoldByte == 33) byteArray[i] = 99;

                if (HoldByte == 100) byteArray[i] = 72;
                if (HoldByte == 72) byteArray[i] = 100;

                if (HoldByte == 101) byteArray[i] = 52;
                if (HoldByte == 52) byteArray[i] = 101;

                if (HoldByte == 102) byteArray[i] = 75;
                if (HoldByte == 75) byteArray[i] = 102;

                if (HoldByte == 103) byteArray[i] = 78;
                if (HoldByte == 78) byteArray[i] = 103;

                if (HoldByte == 104) byteArray[i] = 104;
                if (HoldByte == 46) byteArray[i] = 46;

                if (HoldByte == 105) byteArray[i] = 79;
                if (HoldByte == 79) byteArray[i] = 105;

                if (HoldByte == 106) byteArray[i] = 34;
                if (HoldByte == 34) byteArray[i] = 106;
                
                if (HoldByte == 107) byteArray[i] = 84;
                if (HoldByte == 84) byteArray[i] = 107;
                
                if (HoldByte == 108) byteArray[i] = 65;
                if (HoldByte == 65) byteArray[i] = 108;
                
                if (HoldByte == 109) byteArray[i] = 74;
                if (HoldByte == 74) byteArray[i] = 109;
                
                if (HoldByte == 110) byteArray[i] = 50;
                if (HoldByte == 50) byteArray[i] = 110;
                
                if (HoldByte == 111) byteArray[i] = 67;
                if (HoldByte == 67) byteArray[i] = 111;
                
                if (HoldByte == 112) byteArray[i] = 41;
                if (HoldByte == 41) byteArray[i] = 112;
                
                if (HoldByte == 113) byteArray[i] = 68;
                if (HoldByte == 68) byteArray[i] = 113;
                
                if (HoldByte == 114) byteArray[i] = 64;
                if (HoldByte == 64) byteArray[i] = 114;
                
                if (HoldByte == 115) byteArray[i] = 73;
                if (HoldByte == 73) byteArray[i] = 115;
                
                if (HoldByte == 116) byteArray[i] = 47;
                if (HoldByte == 47) byteArray[i] = 116;
                
                if (HoldByte == 117) byteArray[i] = 63;
                if (HoldByte == 63) byteArray[i] = 117;
                
                if (HoldByte == 118) byteArray[i] = 85;
                if (HoldByte == 85) byteArray[i] = 118;
                
                if (HoldByte == 119) byteArray[i] = 86;
                if (HoldByte == 86) byteArray[i] = 119;
                
                if (HoldByte == 120) byteArray[i] = 35;
                if (HoldByte == 35) byteArray[i] = 120;
                
                if (HoldByte == 121) byteArray[i] = 61;
                if (HoldByte == 61) byteArray[i] = 121;

                if (HoldByte == 122) byteArray[i] = 60;
                if (HoldByte == 60) byteArray[i] = 122;
                
                if (HoldByte == 123) byteArray[i] = 54;
                if (HoldByte == 54) byteArray[i] = 123;
                
                if (HoldByte == 124) byteArray[i] = 76;
                if (HoldByte == 76) byteArray[i] = 124;
                
                if (HoldByte == 125) byteArray[i] = 51;
                if (HoldByte == 51) byteArray[i] = 125;

                if (HoldByte == 126) byteArray[i] = 32; 
                if (HoldByte == 32) byteArray[i] = 126;
                
                if (HoldByte == 69) byteArray[i] = 55;
                if (HoldByte == 55) byteArray[i] = 69;
                
                if (HoldByte == 70) byteArray[i] = 38;
                if (HoldByte == 38) byteArray[i] = 70;
                
                if (HoldByte == 71) byteArray[i] = 71;
                if (HoldByte == 42) byteArray[i] = 42;

                if (HoldByte == 77) byteArray[i] = 36;
                if (HoldByte == 36) byteArray[i] = 77;

                if (HoldByte == 80) byteArray[i] = 57;
                if (HoldByte == 57) byteArray[i] = 88;
                
                if (HoldByte == 81) byteArray[i] = 45;
                if (HoldByte == 45) byteArray[i] = 81;
                
                if (HoldByte == 83) byteArray[i] = 43;
                if (HoldByte == 43) byteArray[i] = 83;
                
                if (HoldByte == 87) byteArray[i] = 48;
                if (HoldByte == 48) byteArray[i] = 87;
                
                if (HoldByte == 80) byteArray[i] = 39;
                if (HoldByte == 39) byteArray[i] = 80;
            }
            return byteArray;
        }
    }
}

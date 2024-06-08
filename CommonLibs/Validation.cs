using System.Text.RegularExpressions;

namespace CommonLibs
{
    public partial class Validation
    {
        public static bool IsNumber(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return false;

            if (!input.All(char.IsDigit))
                return false;

            return true;
        }

        public static bool IsName(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (IsContainHTMLTags(input))
                return false;

            if (!VietnameseNameCharacters().IsMatch(input))
                return false;

            return true;
        }

        public static bool IsContainNumber(string input)
        {
            return input.Any(char.IsDigit);
        }

        public static bool IsContainSpecialCharacters(string input)
        {
            // https://stackoverflow.com/a/2522949
            char[] SpecialChars = "!@#$%^&*()".ToCharArray();
            int indexOf = input.IndexOfAny(SpecialChars);

            return indexOf != -1;
        }

        public static bool IsContainHTMLTags(string input)
        {
            return HTMLTags().IsMatch(input);
        }

        public static bool IsValidEmail(string email)
        {
            // https://stackoverflow.com/a/1374644
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        [GeneratedRegex(@"<[^>]+>")]
        private static partial Regex HTMLTags();

        // https://stackoverflow.com/a/46265018
        [GeneratedRegex(@"^([a-zA-Z]|\s|[àáãạảăắằẳẵặâấầẩẫậèéẹẻẽêềếểễệđìíĩỉịòóõọỏôốồổỗộơớờởỡợùúũụủưứừửữựỳỵỷỹýÀÁÃẠẢĂẮẰẲẴẶÂẤẦẨẪẬÈÉẸẺẼÊỀẾỂỄỆĐÌÍĨỈỊÒÓÕỌỎÔỐỒỔỖỘƠỚỜỞỠỢÙÚŨỤỦƯỨỪỬỮỰỲỴỶỸÝ])+$")]
        private static partial Regex VietnameseNameCharacters();
    }
}

using System.Text.RegularExpressions;

namespace PhoneShop.Commonlibs
{
    public static partial class Validation
    {
        public static bool IsNumber(this string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (!input.All(char.IsDigit))
                return false;

            return true;
        }

        public static bool IsName(this string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (IsContainHTMLTags(input))
                return false;

            if (!VietnameseNameCharacters().IsMatch(input))
                return false;

            return true;
        }

        public static bool IsContainNumber(this string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return input.Any(char.IsDigit);
        }

        public static bool IsContainSpecialCharacters(this string? input)
        {
            // https://stackoverflow.com/a/2522949
            char[] SpecialChars = "!@#$%^&*()".ToCharArray();
            if (string.IsNullOrEmpty(input))
                return false;
            int indexOf = input.IndexOfAny(SpecialChars);

            return indexOf != -1;
        }
        public static bool IsContainHTMLTags(this string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return HTMLTags().IsMatch(input);
        }
        public static bool IsValidEmail(this string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // https://stackoverflow.com/a/1374644
            var trimmedEmail = input.Trim();

            if (trimmedEmail.EndsWith('.'))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
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

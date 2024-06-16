using System.Text;

namespace SucessPointCore.Domain.Helpers
{
    public class NumberGenerator
    {
        public string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomStringBuilder = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomStringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return randomStringBuilder.ToString();
        }
    }
}

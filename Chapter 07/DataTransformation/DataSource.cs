using System.Text;

namespace DataTransformation
{
    class DataSource
    {
        private string[] firstNames = ["John", "Sally", "Deborah",
            "Frank", "Robert", "Giles", "Nancy", "Vincent", "Lisa",
            "Leslie", "Gus", "Mary", "Bruce"];
        private string[] lastNames = ["Brown", "Smith",
            "Corey", "Johnson", "Michaels", "Durant",
            "Mitchel", "Donovan", "O'Riely", "McConnel"];

        public IEnumerable<string> GetData()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var nameBuilder = new StringBuilder();
                nameBuilder.Append(GetRandomElement(firstNames));
                if (Random.Shared.Next(0, 10) < 3)
                    nameBuilder.Append(" " + GetRandomElement(firstNames));
                nameBuilder.Append(" " + GetRandomElement(lastNames));

                yield return nameBuilder.ToString();
            }
        }

        private string GetRandomElement(string[] array)
        {
            return array[Random.Shared.Next(0, array.Length)];
        }
    }
}

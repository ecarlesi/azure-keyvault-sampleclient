namespace SampleKeyVaultClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SkvClient client = new SkvClient();

            string key = "MyKey";

            client.Set(key, "Simple value");

            string value = client.Get(key);

            client.Delete(key);
        }
    }
}

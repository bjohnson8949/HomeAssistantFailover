namespace HomeAssistantFailover
{
    public class HAFConfiguration
    {
        public HAFConfiguration()
        {
            Load();
        }

        string homeAssistantIP { get; set; }
        string deCONZIP  { get; set; }
        string deCONZPort  { get; set; }
        bool RunAsService  { get; set; }
        int RunFrequncy  { get; set; }
        string APIKey  { get; set; }

        private void Save()
        {

        }

        private void Load()
        {

        }
    }
}
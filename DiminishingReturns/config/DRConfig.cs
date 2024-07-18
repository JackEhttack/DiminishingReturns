using BepInEx.Configuration;

namespace JackEhttack;

class DRConfig
{
    public readonly ConfigEntry<int> restock;
    public readonly ConfigEntry<float> denominator;
    public readonly ConfigEntry<float> maxBonus;
    public readonly ConfigEntry<float> bonusChance;

    public DRConfig(ConfigFile cfg)
    {
        // We want to disable saving our config file every time we bind a
        // setting as it's inefficient and slow
        cfg.SaveOnConfigSet = false; 
        
        restock = cfg.Bind(
            "General",
            "DaysTillRestock",
            4,
            "How many days until a moon with diminishing returns regenerate their scrap to full.");

        denominator = cfg.Bind(
            "General",
            "MaxDiminish",
            0.5f,
            "The maximum amount a moon's scrap can be diminished.");
        
        maxBonus = cfg.Bind(
            "General",
            "MaxBonus",
            3.2f,
            "The upper limit for a bonus moon's scrap.");

        bonusChance = cfg.Bind(
            "General",
            "BonusChance",
            0.5f,
            "The chance of a bonus moon appearing per day.");
    }

}
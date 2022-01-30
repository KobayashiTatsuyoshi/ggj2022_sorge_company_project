using UnityEngine;

namespace SorgeProject.Data
{
    [System.Serializable]
    public class InfoData : Util.ISerializableData
    {
        public int id;
        public string title;
        public int power;
        public int moral;
        public int trust;
        public int price;
        public float profit;
        public string flavor;
        public string type;
        public int event_id;

        public void Set(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = int.Parse(value);
                    return;
                case "title":
                    title = value;
                    return;
                case "power":
                    power = int.Parse(value);
                    return;
                case "price":
                    price = int.Parse(value);
                    return;
                case "moral":
                    moral = int.Parse(value);
                    return;
                case "trust":
                    trust = int.Parse(value);
                    return;
                case "profit":
                    profit = float.Parse(value);
                    return;
                case "flavor":
                    flavor = value;
                    return;
                case "class":
                    type = value;
                    return;
                case "event_id":
                    event_id = int.Parse(value);
                    return;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        public const int UNSELLABLE_ITEM_PRICE = -1;

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_IRON_SWORD = 11;

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Старый Меч", "Старые Мечи", 0, 5,5,"Урон: 0 - 5 очков "));
            Items.Add(new Weapon(ITEM_ID_IRON_SWORD, "Железный Меч", "Железный Мечи", 2, 9, 10, "Урон: 0 - 10 очков"));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Крысиный хвост", "Крысиных хвостов",3));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Кусок меха", "Куски меха",2));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Змеиный клык", "Змеинных клыков",2));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Змеиная шкура", "Змеинных шкур",3));
            //Items.Add(new Weapon(ITEM_ID_CLUB, "Дубинка", "Дубинки", 3, 10, 4, "Урон: 0 - 5 "));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Зелье Лечение", "Зельи Лечения", 5,2));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Паучие клык", "Паучих клыков",2));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Шелк паука", "Шелки паука",2));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Пропуск Авентюриста", "Пропуски Авентюриста", UNSELLABLE_ITEM_PRICE));
        }

        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Крыс", 5, 3, 10, 4, 3, "Моб: Крыса- Урон 0-5,Здоровье 4 очков ","rat.jpg");
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_HEALING_POTION), 75, true));


            Monster snake = new Monster(MONSTER_ID_SNAKE, "Змея", 5, 3, 10, 5, 3, "Моб: Змея- Урон 0-5, Здоровье 5 очков","snake.jpg");
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Гигантский Паук", 20, 5, 40, 10, 10, "Моб: Гигантски Паук- Урон 0-20, Здоровье 40 очков ","Giant_spider.png");
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
        }

        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "'Очистить сад Алхимика'",
                    "Убейте крыс в саду алхимика и верните 3 Крысинного хвоста. Вы получите целительное зелье, 10 золотых монет и железный меч", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);
            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_IRON_SWORD);
            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "'Очистить поле фермеру'",
                    "Убейте змей на поле фермера и верните 3 клыков snake-а . Вы получите пропуск авантюриста и 20 золотых монет", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
        }

        private static void PopulateLocations()
        {
            // Создать каждое местоположение
            Location home = new Location(LOCATION_ID_HOME, "Дом", "Твой дом. Тебе действительно нужно навести порядок.", "2_1.jpg");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Городская площадь ", " Вы видите фонтан.", "1_1.jpg");



            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Хижина алхимика", "На полках много странных растений", "2_2.3.jpg");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Алхимический сад ", " Здесь растут многие растения.", "сад_алхимика.jpg");
            alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Сельский дом ", " Есть небольшой сельский дом, с фермером впереди.", "сельский_дом.jpg");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Фермерское поле ", " Вы видите растущие здесь ряды овощей.", "фермер2.jpg");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Охранный пост "," Здесь большой, крепкий на вид охранник.","охранник.jpg", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Мост ", " Каменный мост пересекает широкую реку.", "мост3.jpg");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Лес ", " Вы видите паутину, покрывающую деревья в этом лесу", "лес.jpg");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            // Свяжите места вместе
            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            // Добавьте местоположения в статический список
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }


        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }

            return null;
        }

      

        public static Location LocationByID(int id)
        {

            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }
    }
}
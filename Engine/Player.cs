using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level {
            get { return ((ExperiencePoints / 100) + 1); }
        }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints)
            : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                // Для этого местоположения нет обязательного элемента, поэтому верните "true"
                return true;
            }

            // Посмотрите, есть ли у игрока необходимый предмет в его инвентаре
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    // Мы нашли нужный товар, поэтому вернем "true"
                    return true;
                }
            }

            // Мы не нашли нужный предмет в их инвентаре, поэтому верните «false»
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // Посмотрите, есть ли у игрока все предметы, необходимые для выполнения квеста здесь
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;

                // Проверьте каждый предмет в инвентаре игрока, чтобы увидеть, есть ли он у него и достаточно ли этого.
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID) // У игрока есть предмет в инвентаре.
                    {
                        foundItemInPlayersInventory = true;

                        if (ii.Quantity < qci.Quantity) // У игрока недостаточно этого предмета, чтобы выполнить квест.
                        {
                            return false;
                        }
                    }
                }

                // У игрока нет этого предмета завершения квеста в его инвентаре.
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }

            // Если мы попали сюда, то у игрока должны быть все необходимые предметы, и их достаточно, чтобы выполнить квест.
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        //Вычтите количество из инвентаря игрока, необходимое для выполнения квеста.
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == itemToAdd.ID)
                {
                    // У них есть предмет в инвентаре, поэтому увеличьте количество на один
                    ii.Quantity++;

                    return; //Мы добавили пункт, и все готово, так что выйти из этой функции
                }
            }

            // У них не было предмета, поэтому добавьте его в свой инвентарь, с количеством 1.
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            // Найдите квест в списке квестов игрока.
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    // Отметьте как выполненное.
                    pq.IsCompleted = true;

                    return; //Мы нашли квест и отметили его как завершенный, поэтому выйдите из этой функции.
                }
            }
        }
    }
}
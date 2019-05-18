using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;
  
namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        

        public SuperAdventure()
        {
            InitializeComponent();
            
            _player = new Player(10, 10, 20, 0);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
  
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();

           

        }
  
        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }
  
        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }
  
        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }
  
        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }
  


        private void MoveTo(Location newLocation)
        {
            //Есть ли у местоположения какие-либо обязательные пункты
            if(!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "Вы должны иметь " + newLocation.ItemRequiredToEnter.Name + "чтобы войти в это место." + Environment.NewLine;
                return;
            }

            // Обновить текущее местоположение игрока.
            _player.CurrentLocation = newLocation;


            // Показать / скрыть доступные кнопки движения.
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);
  
            // Показать название текущего местоположения и описание.
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine; 

            //Показать фото локаций 

            pictureBox1.ImageLocation = newLocation.PictureName;
            

            // Полностью исцелить игрока
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Обновление очков жизни в интерфейсе
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // У локации есть квест?
            if(newLocation.QuestAvailableHere != null)
            {
                // Посмотрите, есть ли у игрока квест, и выполнили ли они его
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);
  
                // Посмотрите, есть ли у игрока квест.
                if(playerAlreadyHasQuest)
                {
                    // Если игрок еще не завершил квест.
                    if(!playerAlreadyCompletedQuest)
                    {
                        // Посмотрите, есть ли у игрока все предметы, необходимые для выполнения квеста.
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);
  
                        // У игрока есть все предметы, необходимые для выполнения квеста.
                        if(playerHasAllItemsToCompleteQuest)
                        {
                            // Показать сообщение.
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "Вы завершили '" + newLocation.QuestAvailableHere.Name + "' квест." + Environment.NewLine;

                            // Удалить квестовые предметы из инвентаря.
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);
  
                            // Дайте награды за выполнение заданий
                            rtbMessages.Text += "Вы получите: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;
  
                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            // Добавьте предмет награды в инвентарь игрока
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Отметить квест как выполненный.
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // У игрока еще нет квеста
  
                    // Показать сообщения
                    rtbMessages.Text += "Вы получаете задание:" + newLocation.QuestAvailableHere.Name + "" + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "Для завершения вернитесь с: " + Environment.NewLine;

                    foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Добавить квест в список квестов игрока
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }
  
            // У локации есть монстр?
            if(newLocation.MonsterLivingHere != null)
            {
                

                rtbMessages.Text += "Вы видите " + newLocation.MonsterLivingHere.Name + Environment.NewLine;
  
                // Создайте нового монстра, используя значения из стандартного монстра в списке World.Monster.
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);
               
                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints,
                    standardMonster.Description,standardMonster.PictureName);
                
  
                foreach(LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }
  
                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                textBox1.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;
  
                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                textBox1.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }
            

            //Обновить список инвентаря игрока
            UpdateInventoryListInUI();
  
            // Обновить список квестов игрока
            UpdateQuestListInUI();

            // Обновите поле со списком оружия игрока.
            UpdateWeaponListInUI();

            // Обновите список зелий игрока.
            UpdatePotionListInUI();
        }
  
        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;
  
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Имя";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Количество";
  
            dgvInventory.Rows.Clear();
  
            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }
  
        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;
  
            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Имя";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Готово?";
  
            dgvQuests.Rows.Clear();
  
            foreach(PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }
  
        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();
  
            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is Weapon)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

  
            if(weapons.Count == 0)
            {
                // У игрока нет оружия, поэтому скройте поле со списком оружия и кнопку «Использовать»
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
  
                cboWeapons.SelectedIndex = 0;
            }
        }
  
        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();
  
            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }
  
            if(healingPotions.Count == 0)
            {
                // У игрока нет зелий, поэтому спрячьте поле со списком зелий и кнопку «Использовать»
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
  
                cboPotions.SelectedIndex = 0;
            }
        }
  
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Получить текущее выбранное оружие из ComboBox cboWeapons
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            
            // Определите количество урона, наносимого монстру
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            // Нанесите урон монстрам CurrentHitPoints
            _currentMonster.CurrentHitPoints -= damageToMonster;
            // Информация о противниках и  оружьи
            textBox1.Text = _currentMonster.Description + "\n\r";

            // Показать сообщение
            rtbMessages.Text += "Вы ударили " + _currentMonster.Name + " в " + damageToMonster.ToString() + " очков" + Environment.NewLine;

            // Проверьте, мертв ли ​​монстр
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                // Монстр мертв
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "Вы победили " + _currentMonster.Name + Environment.NewLine;

                // Дайте игроку очки опыта за убийство монстра
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.Text += "Вы получите " + _currentMonster.RewardExperiencePoints.ToString() + " очки опыта" + Environment.NewLine;

                // Дайте игроку золото за убийство монстра 
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += "Вы получите " + _currentMonster.RewardGold.ToString() + " золото" + Environment.NewLine;

                // Получить случайные предметы от монстра.
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Добавьте элементы в список lootedItems, сравнив случайное число с процентом выпадения.
                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                // Если ни один предмет не был выбран случайным образом, добавьте предмет(ы) добычи по умолчанию.
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }
                
                // Добавьте украденные предметы в инвентарь игрока.
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "Вы нашли " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "Вы нашли " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }

                //Обновите информацию об игроке и средства управления инвентарем.
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                // Добавьте пустую строку в окно сообщений, только для внешнего вида.
                rtbMessages.Text += Environment.NewLine;

                // Переместить игрока в текущую локацию (вылечить игрока и создать нового монстра для боя).
                MoveTo(_player.CurrentLocation);
            }
            else
            {
                // Монстр все еще жив

                // Определите количество урона, которое монстр наносит игроку.
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

                // Показать сообщение
                rtbMessages.Text += "" + _currentMonster.Name + " нанес " + damageToPlayer.ToString() + " очков повреждений" + Environment.NewLine;

                // Вычесть урон от игрока
                _player.CurrentHitPoints -= damageToPlayer;

                // Обновить данные игрока в пользовательском интерфейсе
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();

                if (_player.CurrentHitPoints <= 0)
                {
                    // Показать сообщение
                    rtbMessages.Text += "" + _currentMonster.Name + " убил Тебя." + Environment.NewLine;

                    // Переместить игрока в "Домой"
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }
  
        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Получить текущее выбранное зелье из выпадающего списка.
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // Добавить количество исцеления к текущим хитовым очкам игрока
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            // CurrentHitPoints не могут превышать MaximumHitPoints игрока.
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            // Удалите зелье из инвентаря игрока
            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            // Показать сообщение
            rtbMessages.Text += "Вы выпили " + potion.Name + Environment.NewLine;

            // Чудовище получает свою очередь атаковать

            // Определите количество урона, которое монстр наносит игроку.
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            // Показать сообщение
            rtbMessages.Text += "" + _currentMonster.Name + " Получил " + damageToPlayer.ToString() + " очки урона." + Environment.NewLine;

            // Вычесть урон от игрока
            _player.CurrentHitPoints -= damageToPlayer;

            if (_player.CurrentHitPoints <= 0)
            {
                // Показать сообщение
                rtbMessages.Text += "" + _currentMonster.Name + " Убил Тебя" + Environment.NewLine;

                // Переместить игрока в "Домой"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            // Обновить данные игрока в пользовательском интерфейсе
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuperAdventure_Load(object sender, EventArgs e)
        {
            
        }

        private void средневековоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottomOfMessages(); //autoscroll
        }
        private void ScrollToBottomOfMessages()
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void btnTrade_Click(object sender, EventArgs e)
        {
            TradingScreen tradingScreen = new TradingScreen();
            tradingScreen.CurrentPlayer = _player;
            tradingScreen.StartPosition = FormStartPosition.CenterParent;
            tradingScreen.ShowDialog(this);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}

       
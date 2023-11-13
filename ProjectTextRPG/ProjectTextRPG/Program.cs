using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using ConsoleTables;

namespace ProjectTextRPG
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "SPARTA_TEXT_RPG";//콘솔창 Title변경
            Character player = new Character("Chad", "전사", 1, 10, 5, 100, 4000);
            Item playerItem = new Item();
            List<Item> list = new List<Item>();//아이템을 담을 List지정
            List<Item> shopItems = new List<Item>();
            shopItems.Add(new Item("수련자 갑옷","수련에 도움을 주는 갑옷입니다.",5,"방어력",false, 1000, false));
            shopItems.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 5, "방어력", false, 2000, false));
            shopItems.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 15, "방어력", false, 3500, false));
            shopItems.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 2, "공격력", false, 600, false));
            shopItems.Add(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 5, "공격력", false, 1500, false));
            shopItems.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2, "공격력", false, 3500, false));
            GameManager gameStrat = new GameManager();
            gameStrat.Start(player, list, playerItem, shopItems);
        }
    }
    public class GameManager
    {
        StringBuilder sb = new StringBuilder();
        ConsoleKeyInfo _keyInfo;
        ConsoleTable? invenTable; //인벤토리 테이블
        ConsoleTable? shopTable; //상점 테이블

        bool isShopORInven = false;//상점 또는 인벤토리의 장착과 구매로 들어갔는지
        public void Start(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)
        {
            Console.Clear();
            sb.Clear(); // 넘어올 때 마다 값 초기화
            sb.AppendLine("스파르타 마을에 오신 여러분 환영합니다.");
            sb.AppendLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(sb.ToString());
            sb.Clear();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("1. [상 태 보 기]");
            sb.AppendLine("2. [인 벤 토 리]");
            sb.AppendLine("3. [상 점]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요");
            sb.Append(">>");
            Console.Write(sb.ToString());

            _keyInfo = Console.ReadKey(true);
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D1:
                    StatusMap(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D2:
                    InvenMap(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D3:
                    ShopMap(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000); //1초 후
                    Start(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void StatusMap(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)//상태 창
        {
            Console.Clear();
            sb.Clear();
            sb.AppendLine("[상 태 보 기]");
            sb.AppendLine("캐릭터의 정보가 표시됩니다.");
            sb.AppendLine();
            sb.AppendLine($"Lv. {player.Level}");
            sb.AppendLine($"{player.Name}({player.Job})");
            sb.AppendLine($"방어력 : {player.Def} {((player.Def - 5 == 0) ? "" : $"+ {player.Def - 5}")}");
            sb.AppendLine($"공격력 : {player.Atk} {((player.Atk - 10 == 0) ? "" : $"+ {player.Atk - 10}")}");
            sb.AppendLine($"체력 : {player.Hp}");
            sb.AppendLine($"Gold : {player.Gold} G");
            sb.AppendLine();
            sb.AppendLine("0. [나 가 기]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());

            _keyInfo = Console.ReadKey(true);
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D0:
                    Start(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    StatusMap(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void InvenMap(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)//인벤토리
        {
            Console.Clear();
            sb.Clear();
            invenTable = new ConsoleTable("아이템 이름", "효과", "아이템 설명", "판매 금액");
            playeritem.ItemTable(invenItems, invenTable, isShopORInven);
            sb.AppendLine("[인 벤 토 리]");
            sb.AppendLine("보유 중인 아이템을 관리할 수 있습니다.");
            sb.AppendLine();
            sb.AppendLine("[아이템 목록]");
            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            invenTable?.Write();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("1. [장 착 관 리]");
            sb.AppendLine("0. [나 가 기]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());
            _keyInfo = Console.ReadKey(true);
            switch (_keyInfo.Key) 
            {
                case ConsoleKey.D1:
                    isShopORInven = true;
                    InvenEquipMap(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D0:
                    Start(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    InvenMap(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void InvenEquipMap(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)//인벤토리
        {
            Console.Clear();
            invenTable = new ConsoleTable("아이템 이름", "효과", "아이템 설명","판매 금액");
            playeritem.ItemTable(invenItems, invenTable, isShopORInven);
            sb.Clear();
            sb.AppendLine("[인 벤 토 리 - 장 착 관 리]");
            sb.AppendLine("숫자를 눌러 아이템을 장착하세요.");
            sb.AppendLine();
            sb.AppendLine("[아이템 목록]");
            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            invenTable?.Write();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("9. [이 름 정 렬]");
            sb.AppendLine("0. [인 벤 토 리]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());
            _keyInfo = Console.ReadKey(true);
            void itemEquip(int index)
            {
                if (invenItems.Count > index)
                {
                    player.OneItemEquip(invenItems, player, index, playeritem);
                    InvenEquipMap(player, invenItems, playeritem, shopItems);
                }
                else
                {
                    Console.WriteLine("잘못 누르셨습니다.");
                    Thread.Sleep(1000);
                    InvenEquipMap(player, invenItems, playeritem, shopItems);
                }
            }
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D1: //1번 아이템
                    itemEquip(0);
                    break;
                case ConsoleKey.D2: //2번 아이템
                    itemEquip(1);
                    break;
                case ConsoleKey.D3: //3번 아이템
                    itemEquip(2);
                    break;
                case ConsoleKey.D4: //4번 아이템
                    itemEquip(3);
                    break;
                case ConsoleKey.D5: //5번 아이템
                    itemEquip(4);
                    break;
                case ConsoleKey.D6: //6번 아이템
                    itemEquip(5);
                    break;
                case ConsoleKey.D9: //뒤로가기
                    invenItems = invenItems.OrderBy(x => x.Name).ToList();//아이템 정렬
                    InvenEquipMap(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D0: //뒤로가기
                    isShopORInven = false;
                    InvenMap(player, invenItems, playeritem, shopItems);
                    break;
                default://1초후 화면 재설정
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    InvenEquipMap(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void ShopMap(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)//상점
        {
            Console.Clear();
            sb.Clear();
            shopTable = new ConsoleTable("아이템 이름", "효과", "아이템 설명", "금액");
            playeritem.ShopItemTable(shopItems, shopTable, isShopORInven);
            sb.AppendLine("[상 점]");
            sb.AppendLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            sb.AppendLine();
            sb.AppendLine("[보유 골드]");
            sb.AppendLine($"{player.Gold} G");
            sb.AppendLine();
            sb.AppendLine("[아이템 목록]");
            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            shopTable?.Write();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("1. [아 이 템 구 매]");
            sb.AppendLine("2. [아 이 템 판 매]");
            sb.AppendLine("0. [나 가 기]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());
            _keyInfo = Console.ReadKey(true);
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D1:
                    isShopORInven = true;
                    ShopItemBuy(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D2:
                    isShopORInven = true;
                    ShopItemSell(player, invenItems, playeritem, shopItems);
                    break;
                case ConsoleKey.D0:
                    Start(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ShopMap(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void ShopItemBuy(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)
        {
            Console.Clear();
            sb.Clear();
            shopTable = new ConsoleTable("아이템 이름", "효과", "아이템 설명", "금액");
            playeritem.ShopItemTable(shopItems, shopTable, isShopORInven);
            sb.AppendLine("[상 점] - 아이템 구매");
            sb.AppendLine("숫자를 눌러 구매할 아이템을 선택하시오.");
            sb.AppendLine();
            sb.AppendLine("[보유 골드]");
            sb.AppendLine($"{player.Gold} G");
            sb.AppendLine();
            sb.AppendLine("[아이템 목록]");
            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            shopTable?.Write();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("0. [나 가 기]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());
            _keyInfo = Console.ReadKey(true);
            void itemSell(int index)
            {
                playeritem.ItemBuy(shopItems, invenItems, player, index);
                ShopItemBuy(player, invenItems, playeritem, shopItems);
            }
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D1:
                    itemSell(0);
                    break;
                case ConsoleKey.D2:
                    itemSell(1);
                    break;
                case ConsoleKey.D3:
                    itemSell(2);
                    break;
                case ConsoleKey.D4:
                    itemSell(3);
                    break;
                case ConsoleKey.D5:
                    itemSell(4);
                    break;
                case ConsoleKey.D6:
                    itemSell(5);
                    break;
                case ConsoleKey.D0:
                    isShopORInven = false;
                    ShopMap(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ShopItemBuy(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
        public void ShopItemSell(Character player, List<Item> invenItems, Item playeritem, List<Item> shopItems)
        {
            Console.Clear();
            sb.Clear();
            invenTable = new ConsoleTable("아이템 이름", "효과", "아이템 설명", "판매 금액");
            playeritem.ItemTable(invenItems, invenTable, isShopORInven);
            sb.AppendLine("[상 점] - 아이템 판매");
            sb.AppendLine("숫자를 눌러 판매할 아이템을 선택하시오.");
            sb.AppendLine();
            sb.AppendLine("[보유 골드]");
            sb.AppendLine($"{player.Gold} G");
            sb.AppendLine();
            sb.AppendLine("[아이템 목록]");
            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            invenTable?.Write();
            Console.ResetColor();
            sb.AppendLine();
            sb.AppendLine("0. [나 가 기]");
            sb.AppendLine();
            sb.AppendLine("원하시는 행동을 입력해주세요.");
            sb.Append(">>");
            Console.Write(sb.ToString());
            _keyInfo = Console.ReadKey(true);
            void itembuy(int index)
            {
                if (invenItems.Count > index)
                {
                    playeritem.ItemSell(shopItems, invenItems, player, index);
                    ShopItemSell(player, invenItems, playeritem, shopItems);
                }
                else
                {
                    Console.WriteLine("잘못 누르셨습니다.");
                    Thread.Sleep(1000);
                    ShopItemSell(player, invenItems, playeritem, shopItems);
                }
            }
            switch (_keyInfo.Key)
            {
                case ConsoleKey.D1:
                    itembuy(0);
                    break;
                case ConsoleKey.D2:
                    itembuy(1);
                    break;
                case ConsoleKey.D3:
                    itembuy(2);
                    break;
                case ConsoleKey.D4:
                    itembuy(3);
                    break;
                case ConsoleKey.D5:
                    itembuy(4);
                    break;
                case ConsoleKey.D6:
                    itembuy(5);
                    break;
                case ConsoleKey.D0:
                    isShopORInven = false;
                    ShopMap(player, invenItems, playeritem, shopItems);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ShopItemBuy(player, invenItems, playeritem, shopItems);
                    break;
            }
        }
    }
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
        public void StatusPlus(List<Item> items, Character character,int index)//현재 장착한 Item의 리스트에 접근하여 플러스한다
        {
            if (items[index].ItemStatus == "공격력" && items[index].IsEquip == true)
            {
                character.Atk += items[index].PlusItem;
            }
            else if (items[index].ItemStatus == "방어력" && items[index].IsEquip == true)
            {
                character.Def += items[index].PlusItem;
            }
        }
        public void StatusMinus(List<Item> items, Character character, int index)//현재 장착한 Item의 리스트에 접근하여 마이너스한다
        {
            if (items[index].ItemStatus == "공격력" && items[index].IsEquip == false)
            {
                character.Atk -= items[index].PlusItem;
            }
            else if (items[index].ItemStatus == "방어력" && items[index].IsEquip == false)
            {
                character.Def -= items[index].PlusItem;
            }
        }
        public void OneItemEquip(List<Item> items, Character character, int index, Item item)//같은 속성의 아이템을 하나만 끼기위한 함수
        {
            if (items[index].ItemStatus == "공격력")
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ItemStatus == "공격력" && items[i].IsEquip == true)
                    {
                        item.IsItem(items, i);
                        StatusMinus(items, character, i);
                    }
                }
                item.IsItem(items, index);
                StatusPlus(items, character, index);
            }
            else if (items[index].ItemStatus == "방어력")
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ItemStatus == "방어력" && items[i].IsEquip == true)
                    {
                        item.IsItem(items, i);
                        StatusMinus(items, character, i);
                    }
                }
                item.IsItem(items, index);
                StatusPlus(items, character, index);
            }
        }
    }
    public class Item
    {
        public string? Name; //아이템 이름
        public bool IsEquip; //아이템 장착 유 / 무
        public string? ItemStatus; // 아이템 특성 이름
        public string? ItemStory;  // 아이템 스토리
        public int PlusItem;    //아이템 능력치
        public int ItemGold;    //아이템 가격
        public bool IsItemBuy;     //아이템 구매확인
        public Item() { }
        public Item(string Name, string ItemStory, int PlusItem, string ItemStatus, bool IsEquip, int itemGold, bool IsItemBuy) //생성시 아이템의 정보 넣기
        {   
            this.Name = Name;
            this.ItemStatus = ItemStatus;
            this.PlusItem = PlusItem;
            this.ItemStory = ItemStory;
            this.IsEquip = IsEquip;
            this.ItemGold = itemGold;
            this.IsItemBuy = IsItemBuy;
    }
        public void IsItem(List<Item> items, int itemNum)//아이템 장착 true or false
        {
            if (items[itemNum].IsEquip == true)
                items[itemNum].IsEquip = false;
            else
                items[itemNum].IsEquip = true;
        }
        public void ItemTable(List<Item> items, ConsoleTable table, bool isInven)//아이템을 Table에 넣어주기
        {
            for (int i = 0; i < items.Count; i++)
            {
                table?.AddRow($"{((isInven == true)? $"{i + 1}.":"-")}{((items[i].IsEquip == true) ? "[E]" : "")} {items[i].Name}", $"{items[i].ItemStatus} + {items[i].PlusItem}", $"{items[i].ItemStory}", $"{items[i].ItemGold * 0.85f}").Configure(o=>o.EnableCount = false);
            }
        }
        public void ShopItemTable(List<Item> shopitems, ConsoleTable table, bool isShop)//아이템을 Table에 넣어주기
        {
            for (int i = 0; i < shopitems.Count; i++)
            {
                table?.AddRow($"{((isShop == true) ? $"{i + 1}." : "-")} {shopitems[i].Name}", $"{shopitems[i].ItemStatus} + {shopitems[i].PlusItem}", $"{shopitems[i].ItemStory}", $"{((shopitems[i].IsItemBuy == true) ? "구매완료": shopitems[i].ItemGold)} G").Configure(o => o.EnableCount = false);
            }
        }
        public void ItemBuy(List<Item> shopitems, List<Item> invenItems,Character character, int index)
        {   //상점아이템의 구매완료 만들기
            if (shopitems[index].IsItemBuy != true && shopitems[index].ItemGold <= character.Gold)
            {
                character.Gold -= shopitems[index].ItemGold;//아이템의 가격만큼 빼기
                shopitems[index].IsItemBuy = true;
                invenItems.Add(shopitems[index]);
                Console.WriteLine("구매 완료했습니다.");
                Thread.Sleep(1000);
            }
            else if (shopitems[index].IsItemBuy != true && shopitems[index].ItemGold > character.Gold)
            {
                Console.WriteLine("GOLD가 부족합니다.");
                Thread.Sleep(1000);
            }
            else
            { 
                Console.WriteLine("이미 구매한 아이템입니다.");
                Thread.Sleep(1000);
            }
        }
        public void ItemSell (List<Item> shopitems, List<Item> invenItems, Character character, int index)
        {   //상점아이템의 판매완료 만들기
            shopitems[index].IsItemBuy = false; //상점 아이템을 구매가능하게 돌리기
            invenItems[index].IsItemBuy = false; //현재 아이템의 정보를 false로 넘겨주기
            character.Gold = character.Gold + (int)((float)invenItems[index].ItemGold * 0.85f);//아이템의 가격에 85%만큼 더하기
            invenItems.Remove(invenItems[index]);
            Console.WriteLine("판매 완료했습니다.");
            Thread.Sleep(1000);
        }
    }
}
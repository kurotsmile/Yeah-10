using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public GameObject panel_tutorial;
    public GameObject panel_win;
    public Text txt_timer_game_win;
    public GameObject Obj_num_row_prefab;
    public Question_Number question_number;
    public Transform tr_all_item_row_num;
    public GameObject obj_effect_bee_true_left;
    public GameObject obj_effect_bee_true_right;
    public ScrollRect scrollrect_main;
    public AudioSource[] sound;
    private int count_number_true = 0;
    public Animator ani;

    [Header("Tool")]
    public Sprite sp_icon_store;
    public Sprite sp_icon_store_ads;
    public Sprite sp_icon_store_buy;
    public string[] s_name_tool;
    public string[] s_tip_tool;
    public Sprite[] sp_icon_tool;
    public Image[] img_gift;
    public Image img_icon_tool;
    public GameObject obj_button_tool;
    public GameObject[] obj_tool_effect;
    public GameObject obj_Button_Suggest;
    public Slider slider_Suggest;

    private bool is_tool = false;
    private int index_tool = 0;
    private int count_Suggest = 5;
    private Num_Obj obj_num_tool = null;

    [Header("Ui emp")]
    public Text txt_scores_ranks;
    public GameObject Panel_gift;

    [Header("Setting")]
    public Sprite icon_setting_alert;
    public Sprite icon_setting_efect;

    [Header("Effect")]
    public GameObject[] effect_prefab;

    private List<Num_Row> list_row;
    private Carrot.Carrot_Box box_shop;
    private Carrot.Carrot_Box_Btn_Item btn_alert_view;
    private Carrot.Carrot_Box_Btn_Item btn_effect_view;

    private string s_id_item_buy_shop = "";
    private string s_id_item_ads_shop = "";
    private Carrot.Carrot_Window_Msg box_msg_shop;
    private int rank_scores = 0;
    private bool is_effect = true;
    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.shop.onCarrotPaySuccess += this.pay_carrot_success;
        this.carrot.ads.onRewardedSuccess += this.onRewardedSuccess;
        this.GetComponent<Game_pad>().load_gamepad();

        this.rank_scores = PlayerPrefs.GetInt("rank_scores", 0);
        this.txt_scores_ranks.text = this.rank_scores.ToString();

        this.panel_win.SetActive(false);
        this.obj_button_tool.SetActive(false);
        this.Panel_gift.SetActive(true);

        this.question_number.load_data();

        this.list_row = new List<Num_Row>();
        this.obj_effect_bee_true_left.SetActive(false);
        this.obj_effect_bee_true_right.SetActive(false);

        this.carrot.game.load_bk_music(this.sound[7]);

        if (PlayerPrefs.GetInt("is_tutorial", 0) == 0) {
            this.panel_tutorial.SetActive(true);
            this.question_number.pause();
        }
        else this.panel_tutorial.SetActive(false);

        if (PlayerPrefs.GetInt("is_alert", 0) == 0) question_number.is_alert = true; else question_number.is_alert = false;
        if (PlayerPrefs.GetInt("is_effect", 0) == 0) this.is_effect = true; else this.is_effect = false;

        this.Add_rows();
        this.Add_rows();
        this.Add_rows();
        this.Add_rows();

        this.check_count_gift();
        this.show_suggest();
    }

    private void check_exit_app()
    {
        if(this.panel_tutorial.activeInHierarchy)
        {
            this.btn_close_tutorial();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_add_rows()
    {
        this.Add_rows();
        this.play_sound();
        this.carrot.ads.show_ads_Interstitial();
    }

    private void Add_rows()
    {
        GameObject obj_row = Instantiate(this.Obj_num_row_prefab);
        obj_row.transform.SetParent(this.tr_all_item_row_num);
        obj_row.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_row.GetComponent<Num_Row>().row_num = this.list_row.Count;
        obj_row.GetComponent<Num_Row>().load_data();
        this.list_row.Add(obj_row.GetComponent<Num_Row>());
        this.scrollrect_main.verticalNormalizedPosition = -1f;
    }

    public void add_num_obj_to_question(Num_Obj num_obj)
    {
        this.play_sound(0);
        this.Create_effect(1, num_obj.transform.position);
        if (this.is_tool)
            this.Load_effect_tool(num_obj);
        else
            this.question_number.add_numb_obj(num_obj);
    }

    public void set_pos_effect_bee_true(Vector3 pos_n1,Vector3 pos_n2)
    {
        carrot.delay_function(0.6f, ()=>Effect_destroy(pos_n1, pos_n2));
        this.obj_effect_bee_true_left.SetActive(true);
        this.obj_effect_bee_true_right.SetActive(true);
        this.obj_effect_bee_true_left.transform.position = pos_n1;
        this.obj_effect_bee_true_right.transform.position = pos_n2;
    }

    private void Effect_destroy(Vector3 pos_n1,Vector3 pos_n2)
    {
        this.Create_effect(0, pos_n1);
        this.Create_effect(0, pos_n2);
    }

    public void stop_effect_bee_true()
    {
        this.obj_effect_bee_true_left.SetActive(false);
        this.obj_effect_bee_true_right.SetActive(false);
        this.play_sound(1);
        this.check_win_game();
        this.count_number_true++;
        if (this.count_number_true > 9) this.show_tool();
        this.check_count_gift();
    }

    private void check_count_gift()
    {
        for(int i = 0; i < this.img_gift.Length; i++)
        {
            if (this.count_number_true > i)
                this.img_gift[i].color = new Color32(69, 1, 0, 225);
            else
                this.img_gift[i].color = Color.white;
        }
    }

    public void play_sound(int index_sound = 0)
    {
        if(this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void btn_show_setting()
    {
        this.GetComponent<Game_pad>().set_no_gamepad_play();
        this.question_number.pause();
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();

        Carrot.Carrot_Box_Item item_alert = box_setting.create_item_of_top("item_alert");
        item_alert.set_icon(this.icon_setting_alert);
        item_alert.set_title("Result dialog");
        item_alert.set_tip("Enable or disable the game results dialog after each click");
        item_alert.set_act(Act_change_status_alert);
        this.btn_alert_view=item_alert.create_item();
        btn_alert_view.set_color(this.carrot.color_highlight);
        if (this.question_number.is_alert)
            btn_alert_view.set_icon(this.carrot.icon_carrot_visible_off);
        else
            btn_alert_view.set_icon(this.carrot.icon_carrot_visible_on);
        Destroy(btn_alert_view.GetComponent<Button>());

        Carrot.Carrot_Box_Item item_effect = box_setting.create_item_of_top("item_effect");
        item_effect.set_icon(this.icon_setting_efect);
        item_effect.set_title("Effect");
        item_effect.set_tip("Enable or disable in-game effects");
        item_effect.set_act(Act_change_status_effect);

        this.btn_effect_view = item_effect.create_item();
        btn_effect_view.set_color(this.carrot.color_highlight);
        if (this.is_effect)
            btn_effect_view.set_icon(this.carrot.icon_carrot_visible_off);
        else
            btn_effect_view.set_icon(this.carrot.icon_carrot_visible_on);
        Destroy(btn_effect_view.GetComponent<Button>());

        box_setting.set_act_before_closing(act_close_setting);
        box_setting.update_color_table_row();
        box_setting.update_gamepad_cosonle_control();
    }

    public void act_close_setting()
    {
        this.GetComponent<Game_pad>().set_yes_gamepad_play();
        this.question_number.unPause();
        this.play_sound();
    }

    public void btn_remove_ads()
    {
        this.carrot.buy_product(0);
        this.play_sound();
    }

    private void in_app_suggest()
    {
        this.show_suggest();
        if (this.box_shop != null) this.box_shop.close();
    }

    public void btn_show_list_app_carrot()
    {
        this.carrot.show_list_carrot_app();
        this.play_sound();
    }

    public void btn_show_share()
    {
        this.carrot.show_share();
        this.play_sound();
    }

    public void btn_show_rate()
    {
        this.carrot.show_rate();
        this.play_sound();
    }

    public void btn_show_tutorial()
    {
        this.panel_tutorial.SetActive(true);
        question_number.pause();
        this.play_sound();
    }

    public void btn_close_tutorial()
    {
        PlayerPrefs.SetInt("is_tutorial", 1);
        this.panel_tutorial.SetActive(false);
        question_number.unPause();
        this.play_sound();
    }

    private void Act_change_status_alert()
    {
        if (this.question_number.is_alert)
        {
            if (this.btn_alert_view != null) this.btn_alert_view.set_icon(this.carrot.icon_carrot_visible_on);
            question_number.is_alert = false;
            PlayerPrefs.SetInt("is_alert", 1);
        }
        else
        {
            if (this.btn_alert_view != null) this.btn_alert_view.set_icon(this.carrot.icon_carrot_visible_off);
            question_number.is_alert = true;
            PlayerPrefs.SetInt("is_alert", 0);
        }
        this.play_sound();
    }

    private void Act_change_status_effect()
    {
        if (this.is_effect)
        {
            if (this.btn_effect_view != null) this.btn_effect_view.set_icon(this.carrot.icon_carrot_visible_on);
            this.is_effect=false;
            PlayerPrefs.SetInt("is_effect", 1);
        }
        else
        {
            if (this.btn_effect_view != null) this.btn_effect_view.set_icon(this.carrot.icon_carrot_visible_off);
            this.is_effect=true;
            PlayerPrefs.SetInt("is_effect", 0);
        }
        this.play_sound();
    }

    public bool check_row_space_number_true(int index_row, int index_col_n1,int index_col_n2)
    {
        return this.list_row[index_row].check_space_true(index_col_n1, index_col_n2);
    }

    public bool check_col_space_number_true(int index_col, int index_row_n1, int index_row_n2)
    {
        bool is_space_true = true;
        if(index_row_n2> index_row_n1)
        {
            for(int i= index_row_n1+1; i< index_row_n2;i++)
            {
                if (this.list_row[i].check_show_number_col(index_col) != false) is_space_true = false;
            }
        }
        else
        {
            for (int i = index_row_n1 + 1; i < index_row_n2; i++)
            {
                if (this.list_row[i].check_show_number_col(index_col) != false) is_space_true = false;
            }
        }
        return is_space_true;
    }

    private void check_win_game()
    {
        bool is_win = true;
        for(int i = 0; i < this.list_row.Count; i++)
        {
            if (this.list_row[i].check_win_game() == false)
            {
                is_win = false;
                break;
            }
        }

        if (is_win)
        {
            this.panel_win.SetActive(true);
            this.txt_timer_game_win.text = question_number.txt_timer.text;
            this.play_sound(3);
        }
    }

    public void btn_game_replay()
    {
        this.is_tool = false;
        this.obj_button_tool.SetActive(false);
        this.Panel_gift.SetActive(true);
        this.list_row = new List<Num_Row>();
        this.carrot.clear_contain(this.tr_all_item_row_num);
        this.Add_rows();
        this.Add_rows();
        this.Add_rows();
        this.Add_rows();
        this.panel_win.SetActive(false);
        this.question_number.restart();
        this.play_sound();
        this.count_number_true = 0;
        this.check_count_gift();
        this.carrot.ads.show_ads_Interstitial();
        this.show_suggest();
        this.GetComponent<Game_pad>().reset_gamepad();
    }

    public void btn_close_tool()
    {
        this.ani.Play("Game");
        this.play_sound();
        this.obj_button_tool.SetActive(false);
        this.Panel_gift.SetActive(true);
    }

    public void btn_active_tool()
    {
        this.question_number.reset_select();
        this.is_tool = true;
        this.ani.Play("Game_active_tool");
        this.play_sound();
        this.carrot.ads.show_ads_Interstitial();
    }

    private void Load_effect_tool(Num_Obj n)
    {
        this.ani.Play("Game");
        this.obj_button_tool.SetActive(false);
        this.Panel_gift.SetActive(true);
        this.is_tool = false;
        this.obj_num_tool = n;

        if (this.index_tool == 0 || this.index_tool == 1 || this.index_tool == 3) { 
            GameObject obj_effect_tool = Instantiate(this.obj_tool_effect[this.index_tool]);
            obj_effect_tool.transform.SetParent(this.transform);
            obj_effect_tool.transform.localScale = new Vector3(1f, 1f, 1f);
            obj_effect_tool.transform.position = n.transform.position;
            Destroy(obj_effect_tool, 2f);
        }

        if (this.index_tool == 2) for (int i = 0; i < this.list_row.Count; i++) this.list_row[i].effect_boom_by_int(n.int_num);

        if (this.index_tool == 0)
        {
            int index_col_sel = n.col_num;
            for (int i = 0; i < this.list_row.Count; i++) this.list_row[i].get_num_by_col(index_col_sel).select();
        }

        if (this.index_tool == 1)
        {
            int index_row_sel = n.row_num;
            this.list_row[index_row_sel].select();
        }

        if (this.index_tool == 2) for (int i = 0; i < this.list_row.Count; i++) this.list_row[i].select_by_int(n.int_num);

        if (this.index_tool == 3) { 
            n.select();
            this.play_sound(6);
        }
        else
        this.play_sound(4);

        this.carrot.delay_function(2f,this.active_effect_tool);
    }

    public void add_effect_boom(Vector3 pos)
    {
        GameObject obj_effect_tool = Instantiate(this.obj_tool_effect[2]);
        obj_effect_tool.transform.SetParent(this.transform);
        obj_effect_tool.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_effect_tool.transform.position = pos;
        Destroy(obj_effect_tool, 2f);
    }

    private void active_effect_tool()
    {
        if (this.index_tool == 0)
        {
            int index_col_sel = this.obj_num_tool.col_num;
            for (int i = 0; i < this.list_row.Count; i++) this.list_row[i].get_num_by_col(index_col_sel).deactivate();
        }

        if (this.index_tool == 1)
        {
            int index_row_sel = this.obj_num_tool.row_num;
            this.list_row[index_row_sel].deactivate();
        }

        if (this.index_tool == 2) for (int i = 0; i < this.list_row.Count; i++) this.list_row[i].deactivate_by_int(obj_num_tool.int_num);

        if (this.index_tool == 3) this.obj_num_tool.deactivate();

        this.check_win_game();
    }

    private void show_tool(int index_tool=-1)
    {
        if(index_tool==-1) index_tool = Random.Range(0, this.sp_icon_tool.Length-2);
        this.img_icon_tool.sprite = this.sp_icon_tool[index_tool];
        this.index_tool = index_tool;
        this.obj_button_tool.SetActive(true);
        this.count_number_true = 0;
        this.ani.Play("Game_ready_tool");
        this.Panel_gift.SetActive(false);
        this.play_sound(5);
    }

    public void btn_show_shop()
    {
        this.GetComponent<Game_pad>().set_no_gamepad_play();
        this.question_number.pause();
        this.play_sound();
        this.box_shop=this.carrot.Create_Box("box_shop");
        this.box_shop.set_title("Shop");
        this.box_shop.set_icon(this.sp_icon_store);

        int leng_shop = 0;
        if(this.carrot.ads.get_status_ads())
            leng_shop = this.sp_icon_tool.Length;
        else
            leng_shop= this.sp_icon_tool.Length-1;

        for (int i = 0; i < leng_shop; i++)
        {
            Carrot.Carrot_Box_Item shop_item=this.box_shop.create_item("item_shop_"+i);
            var s_id_item_shop = shop_item.name;
            shop_item.set_icon_white(this.sp_icon_tool[i]);
            shop_item.set_title(this.s_name_tool[i]);
            shop_item.set_tip(this.s_tip_tool[i]);
            shop_item.set_act(() => this.sel_item_shop(s_id_item_shop));
            this.create_btn_for_item(shop_item);
        }

        this.box_shop.set_act_before_closing(act_close_shop);
        this.box_shop.update_color_table_row();
        this.box_shop.update_gamepad_cosonle_control();
    }

    private void create_btn_for_item(Carrot.Carrot_Box_Item items)
    {
        Carrot.Carrot_Box_Btn_Item btn_buy=items.create_item();
        btn_buy.set_icon(this.sp_icon_store_buy);
        btn_buy.set_color(this.carrot.color_highlight);
        btn_buy.set_act(() => act_buy_shop_item(items.name));

        if(items.name!= "item_shop_5")
        {
            if (carrot.ads.get_status_ads())
            {
                Carrot.Carrot_Box_Btn_Item btn_ads = items.create_item();
                btn_ads.set_icon(this.sp_icon_store_ads);
                btn_ads.set_color(this.carrot.color_highlight);
                btn_ads.set_act(() => act_ads_shop_item(items.name));
            }
        }
    }

    private void act_buy_shop_item(string id_name_item)
    {
        this.s_id_item_buy_shop = id_name_item;
        this.carrot.play_sound_click();
        if(id_name_item == "item_shop_5")
            this.carrot.buy_product(0);
        else if (id_name_item == "item_shop_4")
            this.carrot.buy_product(2);
        else
            this.carrot.buy_product(1);
    }

    private void act_ads_shop_item(string id_name_item)
    {
        this.s_id_item_ads_shop = id_name_item;
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Rewarded();
    }

    public void act_close_shop()
    {
        this.GetComponent<Game_pad>().set_yes_gamepad_play();
        this.question_number.unPause();
        this.play_sound();
    }

    public void act_item_shop(string id_item_name)
    {
        if (this.box_msg_shop != null) this.box_msg_shop.close();
        if (this.box_shop != null) this.box_shop.close();
        if (id_item_name == "item_shop_0") this.show_tool(0);
        if (id_item_name == "item_shop_1") this.show_tool(1);
        if (id_item_name == "item_shop_2") this.show_tool(2);
        if (id_item_name == "item_shop_3") this.show_tool(3);
        if (id_item_name == "item_shop_4") this.in_app_suggest();
        if (id_item_name == "item_shop_5") this.carrot.ads.remove_ads();
    }

    public void sel_item_shop(string id_item_name)
    {
        if (this.box_msg_shop != null) this.box_msg_shop.close();
        this.box_msg_shop=this.carrot.Show_msg("Shop", "You can buy or watch ads to use this item", Carrot.Msg_Icon.Question);
        this.box_msg_shop.add_btn_msg("Buy",()=> act_buy_shop_item(id_item_name));
        if (carrot.ads.get_status_ads())
        {
            if (id_item_name != "item_shop_5") this.box_msg_shop.add_btn_msg("Watch ads", () => act_ads_shop_item(id_item_name));
        }

        this.box_msg_shop.add_btn_msg("Cancel", close_msg_shop);
    }

    private void close_msg_shop()
    {
        this.carrot.play_sound_click();
        if (this.box_msg_shop != null) this.box_msg_shop.close();
    }

    public void btn_suggest()
    {
        for(int i = 0; i < this.list_row.Count; i++)
        {
            if (this.list_row[i].suggest()) break;
        }
        this.play_sound();
        this.carrot.ads.show_ads_Interstitial();
        this.count_Suggest--;
        if (this.count_Suggest <1)
            this.obj_Button_Suggest.SetActive(false);
        else
            this.slider_Suggest.value = this.count_Suggest;
    }

    private void show_suggest()
    {
        this.count_Suggest = 5;
        this.slider_Suggest.value = this.count_Suggest;
        this.obj_Button_Suggest.SetActive(true);
    }

    public void btn_buy_suggest()
    {
        this.play_sound();
        this.carrot.buy_product(2);
    }

    public List<Num_Row> get_list_row()
    {
        return this.list_row;
    }

    private void pay_carrot_success(string s_id_product)
    {
        if (s_id_product == this.carrot.shop.get_id_by_index(1))
        {
            if (this.s_id_item_buy_shop != "")
            {
                this.act_item_shop(this.s_id_item_buy_shop);
                this.s_id_item_buy_shop = "";
            }
        }
        if (s_id_product == this.carrot.shop.get_id_by_index(2)) this.in_app_suggest();
    }

    private void onRewardedSuccess()
    {
        if (s_id_item_ads_shop != "")
        {
            this.act_item_shop(this.s_id_item_ads_shop);
            this.s_id_item_ads_shop = "";
        }
    }

    public void add_scores_ranks()
    {
        this.rank_scores++; 
        PlayerPrefs.SetInt("rank_scores", this.rank_scores);
        this.txt_scores_ranks.text = this.rank_scores.ToString();
        if (Random.Range(0, 5) == 1)
        {
            this.carrot.game.update_scores_player(this.rank_scores);
        }
    }

    public void btn_user()
    {
        this.carrot.user.show_login();
    }

    public void btn_ranks()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void Create_effect(int index_effect,Vector3 pos)
    {
        if (this.is_effect)
        {
            GameObject obj_effect = Instantiate(this.effect_prefab[index_effect]);
            obj_effect.transform.SetParent(this.transform);
            obj_effect.transform.position = pos;
            obj_effect.transform.localScale = new Vector3(1f, 1f, 1f);
            obj_effect.transform.localRotation = Quaternion.identity;
            Destroy(obj_effect, 3f);
        }
    }

}
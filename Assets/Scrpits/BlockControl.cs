using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block {

    public static int BLOCK_NUM_X = 10;
    public static int BLOCK_NUM_Y = 10;
    public static int BLOCK_COLLISION_SIZE = 33;

    [System.Serializable]
    public struct iPosition {
        public int x;
        public int y;
    }

    public enum COLOR { NONE = -1, WHITE, BLUE, RED};
    public enum OPT { NONE = -1, ROOT, NODE};
    public enum STEP { NONE = -1, IDLE, GRABBED, RELEASED, SOIDED, BLOCK};
    public enum GO { NONE = -1, HORIZONTAL, VERTICAL};

}

public class BlockControl : MonoBehaviour {
    public Block.COLOR color = Block.COLOR.NONE;
    public Block.iPosition i_pos;
    public Block.iPosition root_pos;
    public Block.OPT opt;
    public BoxCollider2D collider = null;
    public Block.GO grab_go;
    public int puzzle_num = 0;

    public Block.STEP step = Block.STEP.NONE;
    public Block.STEP next_step = Block.STEP.NONE;

    public BlockRoot block_root = null;
    public Text[] texts;
    public GameObject[] lines;

    // Use this for initialization
    void Start () {
        this.setColor(this.color);
        collider = this.GetComponent<BoxCollider2D>();
        if (opt == Block.OPT.NONE)
            next_step = Block.STEP.BLOCK;
        else 
            next_step = Block.STEP.IDLE;
        //texts = GetComponent<Transform>().GetComponentsInChildren<Text>();
        //Debug.Log(texts[0].text);
    }
	
	// Update is called once per frame
	void Update () {
        switch (this.step) {
            case Block.STEP.BLOCK:
                return;
        }

		while(this.next_step != Block.STEP.NONE) {
            this.step = this.next_step;
            this.next_step = Block.STEP.NONE;

            switch (this.step) {
                case Block.STEP.IDLE:
                    break;
                case Block.STEP.GRABBED:
                    block_root.changeExpWithIpos(i_pos, grab_go);
                    break;
                case Block.STEP.RELEASED:
                    setColor(Block.COLOR.NONE);
                    break;
                case Block.STEP.BLOCK:
                    setColor(Block.COLOR.NONE);
                    setFirstText("");
                    setSecondText("");
                    break;
                case Block.STEP.SOIDED:
                    puzzle_num--;
                    if (puzzle_num > 0)
                        next_step = Block.STEP.IDLE;
                    break;
            }
        }
	}

    public void setColor(Block.COLOR color) {
        this.color = color;
        Color color_value;
        switch (this.color) {
            default:
            case Block.COLOR.NONE:
                color_value = Color.white;
                color_value.a = 0.0f;
                this.GetComponent<Image>().sprite = null;
                break;
            case Block.COLOR.WHITE:
                color_value = Color.white;
                break;
            case Block.COLOR.BLUE:
                color_value = Color.Lerp(this.GetComponent<Image>().color, Color.blue, 0.5f);
                break;
            case Block.COLOR.RED:
                color_value = Color.red;
                break;
        }
        this.GetComponent<Image>().color = color_value;
    }

    public void beginGrab(Block.GO grab_go = Block.GO.NONE) {
        this.next_step = Block.STEP.GRABBED;
        this.grab_go = grab_go;
    }

    public void endGrab() {
        this.next_step = Block.STEP.IDLE;
    }

    public bool isGrabbable(Block.GO grab_go = Block.GO.NONE) {
        bool is_grabbable= false;
        switch (this.step) {
            case Block.STEP.IDLE:
                is_grabbable = true;
                break;
            case Block.STEP.GRABBED:
                if (grab_go != Block.GO.NONE && this.grab_go != grab_go)
                    is_grabbable = true;
                break;
        }
        return is_grabbable;
    }
    public void intoBlock() {
        this.next_step = Block.STEP.BLOCK;
    }
    public void setFirstText(string s) {
        //texts[0].text = s;
    }
    public void setSecondText(string s) {
        texts[1].text = s;
    }
    public void solvedBlock() {
        next_step = Block.STEP.SOIDED;
    }

    //겉 테두리
    public void activingLine() {
        foreach(GameObject line in lines) {
            //line.SetActive(true);
        }
    }
}

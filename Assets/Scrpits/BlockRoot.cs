using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockRoot : MonoBehaviour {
    public GameObject blockPrefab = null;
    public BlockControl[,] blocks;
    private BlockControl grabbed_block = null;
    private LevelData grabbed_level_data = null;
    private Camera camera;
    public ScoreManager score_manager;
    public AudioClip buzzer;
    public AudioClip no_buzzer;

    public InputField text_field;
    private string answer;
    private bool input_answer;

    private void Awake() {
        
    }

    // Use this for initialization
    void Start () {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        input_answer = false;
	}

    // Update is called once per frame
    void Update() {
        Vector3 mouse_position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) {
            foreach (BlockControl block in this.blocks) {
                if (!block.isGrabbable())
                    continue;
                if (!unprojectCheck(block.collider, mouse_position))
                    continue;
                if (block.opt == Block.OPT.NODE) {
                    BlockControl root_block = this.blocks[block.root_pos.x, block.root_pos.y];
                    if (block.i_pos.x == root_block.i_pos.x) {
                        if (root_block.isGrabbable(Block.GO.VERTICAL)) {
                            if (grabbed_block != null) {
                                this.grabbed_block.endGrab();
                                changeColorWithLeveldata(Block.COLOR.WHITE);
                            }
                            this.grabbed_block = root_block;
                            this.grabbed_block.beginGrab(Block.GO.VERTICAL);
                        }
                    } else {
                        if (root_block.isGrabbable(Block.GO.HORIZONTAL)) {
                            if (grabbed_block != null) {
                                this.grabbed_block.endGrab();
                                changeColorWithLeveldata(Block.COLOR.WHITE);
                            }
                            this.grabbed_block = root_block;
                            this.grabbed_block.beginGrab(Block.GO.HORIZONTAL);
                        }
                    }
                } else {
                    if (grabbed_block != null) {
                        this.grabbed_block.endGrab();
                        changeColorWithLeveldata(Block.COLOR.WHITE);
                    }
                    this.grabbed_block = block;
                    this.grabbed_block.beginGrab();
                }
                break;
            }
        }
        if (input_answer) {
            if (grabbed_level_data != null) {
                //정답확인
                if (answer.Equals(grabbed_level_data.answer)) {
                    changeColorWithLeveldata(Block.COLOR.WHITE);
                    int n = 0;
                    grabbed_block.solvedBlock();
                    grabbed_block.setSecondText(grabbed_level_data.answer[n++].ToString());
                    if (grabbed_level_data.go == Block.GO.HORIZONTAL) {
                        for (int x = grabbed_level_data.i_pos.x + 1; x < grabbed_level_data.i_pos.x + grabbed_level_data.len; x++) {
                            BlockControl bc = this.blocks[x, grabbed_level_data.i_pos.y];
                            if (bc.opt != Block.OPT.ROOT) {
                                bc.solvedBlock();
                            }
                            bc.setSecondText(grabbed_level_data.answer[n++].ToString());
                        }
                    } else {
                        for (int y = grabbed_level_data.i_pos.y + 1; y < grabbed_level_data.i_pos.y + grabbed_level_data.len; y++) {
                            BlockControl bc = this.blocks[grabbed_level_data.i_pos.x, y];
                            if (bc.opt != Block.OPT.ROOT) {
                                bc.solvedBlock();
                            }
                            bc.setSecondText(grabbed_level_data.answer[n++].ToString());
                        }
                    }
                    grabbed_block = null;
                    grabbed_level_data = null;
                    SoundManager.instance.PlaySingle(buzzer);
                    //정답을 틀렸을 때
                } else {
                    SoundManager.instance.PlaySingle(no_buzzer);
                }
            }
            input_answer = false;
        }
    }

    public void initialSetup(List<LevelData> level_datas) {
        this.blocks = new BlockControl[Block.BLOCK_NUM_X, Block.BLOCK_NUM_Y];
        for(int y = 0; y < Block.BLOCK_NUM_Y; y++) {
            for(int x = 0; x < Block.BLOCK_NUM_X; x++) {
                GameObject go = Instantiate(this.blockPrefab) as GameObject;
                BlockControl block = go.GetComponent<BlockControl>();
                this.blocks[x, y] = block;

                block.i_pos.x = x;
                block.i_pos.y = y;
                block.block_root = this;

                Vector3 position = BlockRoot.calcBlockPosition(block.i_pos);
                block.transform.position = position;
                block.name = "block (" + block.i_pos.x.ToString() +
                    ", " + block.i_pos.y.ToString() + ")";
            }
        }
        foreach(LevelData level_data in level_datas) {
            BlockControl blockControl = this.blocks[level_data.i_pos.x, level_data.i_pos.y];
            blockControl.opt = Block.OPT.ROOT;
            blockControl.setFirstText(level_data.num.ToString() + ")");
            blockControl.puzzle_num++;
            blockControl.activingLine();
            if(level_data.go == Block.GO.HORIZONTAL) {
                for(int x = level_data.i_pos.x; x < level_data.i_pos.x + level_data.len; x++) {
                    BlockControl bc = this.blocks[x,level_data.i_pos.y];
                    if(bc.opt != Block.OPT.ROOT) {
                        bc.opt = Block.OPT.NODE;
                        bc.root_pos.x = blockControl.i_pos.x;
                        bc.root_pos.y = blockControl.i_pos.y;
                        bc.activingLine();
                    }
                }
            } else {
                for (int y = level_data.i_pos.y; y < level_data.i_pos.y + level_data.len; y++) {
                    BlockControl bc = this.blocks[level_data.i_pos.x, y];
                    if (bc.opt != Block.OPT.ROOT) {
                        bc.opt = Block.OPT.NODE;
                        bc.root_pos.x = blockControl.i_pos.x;
                        bc.root_pos.y = blockControl.i_pos.y;
                        bc.activingLine();
                    }
                }
            }
        }
    }

    public void changeColorWithLeveldata(Block.COLOR color) {
        if (grabbed_level_data == null)
            return;
        grabbed_block.setColor(color);
        if (grabbed_level_data.go == Block.GO.HORIZONTAL) {
            for (int x = grabbed_level_data.i_pos.x + 1; x < grabbed_level_data.i_pos.x + grabbed_level_data.len; x++) {
                BlockControl bc = this.blocks[x, grabbed_level_data.i_pos.y];
                bc.setColor(color);
            }
        } else {
            for (int y = grabbed_level_data.i_pos.y + 1; y < grabbed_level_data.i_pos.y + grabbed_level_data.len; y++) {
                BlockControl bc = this.blocks[grabbed_level_data.i_pos.x, y];
                bc.setColor(color);
            }
        }
    }

    public static Vector3 calcBlockPosition(Block.iPosition i_pos) {

        Vector3 position = Vector3.zero;

        position.x += ((float)i_pos.x - 9.5f) * Block.BLOCK_COLLISION_SIZE;
        position.y += (-(float)i_pos.y + 3.0f) * Block.BLOCK_COLLISION_SIZE;

        return position;
    }

    public bool unprojectCheck(BoxCollider2D box, Vector3 mouse_position) {
        bool ret = false;

        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(mouse_position), Vector2.zero);

        if (hit.collider == box)
            ret = true;
        return ret;
    }

    public void changeExpWithIpos(Block.iPosition i_pos, Block.GO go) {
        grabbed_level_data =  score_manager.changeExpWithIpos(i_pos, go);
        changeColorWithLeveldata(Block.COLOR.BLUE);
    }

    public void inputAnswer() {
        answer = text_field.text;
        text_field.text = "";
        input_answer = true;
    }
}

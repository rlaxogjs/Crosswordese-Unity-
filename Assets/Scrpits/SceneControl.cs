using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour {
    private BlockRoot block_root = null;
    private LevelControl level_control;
    public TextAsset data;
    private ScoreManager score_manager;

	// Use this for initialization
	void Start () {
        this.level_control = GetComponent<LevelControl>();
        level_control.loadLevelData(data);
        this.block_root = GetComponent<BlockRoot>();
        this.block_root.initialSetup(level_control.level_datas);
        this.score_manager = GetComponent<ScoreManager>();

        score_manager.level_control = this.level_control;
        this.block_root.score_manager = this.score_manager;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}

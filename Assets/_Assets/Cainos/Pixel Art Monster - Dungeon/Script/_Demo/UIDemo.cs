using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class UIDemo : MonoBehaviour
    {
        public CameraFollow cameraFollow;
        [Space]
        public GameObject areaControl;
        public GameObject areaControlExtra;
        public GameObject areaMonster;
        public GameObject areaSkin;
        [Space]
        public TMP_Dropdown dropdownMonster;
        public TMP_Dropdown dropdownSkin;
        [Space]
        public List<MonsterInfo> monsters;
        [Space]
        public PixelMonster monster;

        private bool isUIShown = true;
        private int curMonsterIndex;
        private int curSkinIndex;

        public void ToggleUI()
        {
            isUIShown = !isUIShown;

            areaControl.SetActive(isUIShown);
            areaMonster.SetActive(isUIShown);
            areaSkin.SetActive(isUIShown);
        }

        public void Reset()
        {
            SceneManager.LoadScene(0);
        }

        public void OnMonsterChanged(int index)
        {
            curMonsterIndex = index;
            curSkinIndex = 0;

            //set skin options
            List<string> skinOptions = new List<string>();
            for (int i = 0; i < monsters[index].skins.Count; i++) skinOptions.Add(monsters[index].skins[i].name);
            dropdownSkin.ClearOptions();
            dropdownSkin.AddOptions(skinOptions);

            //extra controll for monster mimic
            areaControlExtra.SetActive(curMonsterIndex == 3);

            LoadMonster();
        }

        public void OnSkinChanged(int index)
        {
            curSkinIndex = index;
            LoadMonster();
        }

        private void LoadMonster()
        {
            Vector3 spawnPos = new Vector3(0.0f, 1.0f, 0.0f);
            int facing = 1;

            if (monster)
            {
                spawnPos = monster.transform.position;
                facing = monster.GetComponent<PixelMonster>().Facing;
                Destroy(monster.gameObject);
            }

            monster = Instantiate(monsters[curMonsterIndex].skins[curSkinIndex].prefab).GetComponent<PixelMonster>();
            monster.transform.position = spawnPos;
            monster.Facing = facing;

            cameraFollow.target = monster.transform;
        }

        public void OnKillRevive()
        {
            var mc = monster.GetComponent<MonsterController>();
            if (mc) mc.IsDead = !mc.IsDead;

            var mfc = monster.GetComponent<MonsterFlyingController>();
            if (mfc) mfc.IsDead = !mfc.IsDead;
        }

        public void OnShowHide()
        {
            monster.IsHiding = !monster.IsHiding;
        }

        public void OnInjureFront()
        {
            monster.InjuredFront();
        }

        public void OnInjureBack()
        {
            monster.InjuredBack();
        }

        private void Start()
        {
            //set monster options
            List<string> monsterOptions = new List<string>();
            for (int i = 0; i < monsters.Count; i++) monsterOptions.Add(monsters[i].name);
            dropdownMonster.AddOptions(monsterOptions);

            OnMonsterChanged(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) OnKillRevive();
            if (Input.GetKeyDown(KeyCode.H)) OnShowHide();
            if (Input.GetKeyDown(KeyCode.LeftBracket)) OnInjureFront();
            if (Input.GetKeyDown(KeyCode.RightBracket)) OnInjureBack();
        }

        [System.Serializable]
        public class MonsterInfo
        {
            public string name;
            public List<MonsterSkinInfo> skins;

            [System.Serializable]
            public class MonsterSkinInfo
            {
                public string name;
                public GameObject prefab;
            }

        }

    }
}

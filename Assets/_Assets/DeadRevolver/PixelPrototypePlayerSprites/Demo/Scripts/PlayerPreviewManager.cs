using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DeadRevolver.PixelPrototypePlayer {
    [System.Serializable]
    public class PlayerPreviewAnimation {
        public string name = "";
        public string animationName = "";
    }

    public class PlayerPreviewManager : MonoBehaviour {
        public GameObject previewPlayer;
        public GameObject propLedge;
        public GameObject propLadder;
        public GameObject propLedgeClimb;
        public GameObject propRocks;
        public Animator gunFX;
        public Animator bulletImpactFX;
        public List<PlayerPreviewAnimation> animations = new List<PlayerPreviewAnimation>();
        public UnityEvent<PlayerPreviewAnimation, int, List<PlayerPreviewAnimation>> onAnimationChanged;
        public UnityEvent onAnimationPrev;
        public UnityEvent onAnimationNext;
        private int _currentAnimation = 0;
        private Animator _anim;
        private Vector2 _startPosition;

        void Start() {
            _anim = previewPlayer.GetComponent<Animator>();
            _startPosition = previewPlayer.transform.position;
            UpdateAnimation();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.A)) {
                PreviousAnimation();
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.D)) {
                NextAnimation();
            }
        }

        private void PreviousAnimation() {
            _currentAnimation--;

            if (_currentAnimation < 0) _currentAnimation = animations.Count - 1;

            onAnimationPrev.Invoke();
            UpdateAnimation();
        }

        private void NextAnimation() {
            _currentAnimation++;

            if (_currentAnimation > animations.Count - 1) _currentAnimation = 0;

            onAnimationNext.Invoke();
            UpdateAnimation();
        }

        private void UpdateAnimation() {
            previewPlayer.transform.position = _startPosition;
            PlayerPreviewAnimation currentAnimation = animations[_currentAnimation];
            _anim.Play(currentAnimation.animationName);
            onAnimationChanged.Invoke(currentAnimation, _currentAnimation, animations);

            if (currentAnimation.name == "Ledge Hang" || currentAnimation.name == "Ledge Climb" || currentAnimation.name == "Wall Slide" || currentAnimation.name == "Wall Jump" || currentAnimation.name == "Wall Climb" || currentAnimation.name == "Wall Climb Idle") {
                propLedge.SetActive(true);
            } else {
                propLedge.SetActive(false);
            }

            if (currentAnimation.name == "Ladder Climb" || currentAnimation.name == "Ladder Climb Finish") {
                propLadder.SetActive(true);
            } else {
                propLadder.SetActive(false);
            }

            if (currentAnimation.name == "Ladder Climb Horizontal") {
                propLedgeClimb.SetActive(true);
            } else {
                propLedgeClimb.SetActive(false);
            }

            if (currentAnimation.name == "Climb Up (Left Hand)" || currentAnimation.name == "Climb Up (Right Hand)" || currentAnimation.name == "Climb Down (Left Hand)" || currentAnimation.name == "Climb Down (Right Hand)" || currentAnimation.name == "Climb Left" || currentAnimation.name == "Climb Right" || currentAnimation.name == "Climb Idle" || currentAnimation.name == "Climb Grab/Land" || currentAnimation.name == "Climb Jump Prepare") {
                propRocks.SetActive(true);
            } else {
                propRocks.SetActive(false);
            }

            if (currentAnimation.animationName == "GunPreview") {
                gunFX.gameObject.SetActive(true);
                bulletImpactFX.gameObject.SetActive(true);
                gunFX.Play("MuzzleFlash");
                bulletImpactFX.Play("BulletImpact");
            } else {
                gunFX.gameObject.SetActive(false);
                bulletImpactFX.gameObject.SetActive(false);
            }
        }
    }
}
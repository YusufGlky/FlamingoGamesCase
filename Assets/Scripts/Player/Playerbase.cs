using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour
{
    [SerializeField] private protected bool moveable;
    [SerializeField] private protected bool victory;

    [Header("Balance")]
    [SerializeField] private protected int leftStick;
    [SerializeField] private protected int rightStick;
    [SerializeField] private float balanceValue=0.5f;

    [Header("Stick")]
    [SerializeField] private Rigidbody stickBody;
    [SerializeField] private List<Transform> leftStickObjects;
    [SerializeField] private List<Transform> rightStickObjects;
    [SerializeField] private Transform leftStickTransform;
    [SerializeField] private Transform rightStickTransform;

    [Header("Shoe")]
    [SerializeField] private GameObject skate;

    [Header("UI")]
    [SerializeField] private TextMeshPro leftStickText;
    [SerializeField] private TextMeshPro rightStickText;

    [Header("ObjectBalance")]
    [SerializeField] private Rigidbody leftStickBody;
    [SerializeField] private Rigidbody rightStickBody;
    #region ConstantVariables
    private protected PlayerValues playerValues;
    private protected ConstantVariables constantVariables;
    #endregion
    #region privateVariables
    private Vector3 _mouseStartPos;
    private Vector3 _mouseEndPos;
    #endregion
    #region Components
    [SerializeField] private Animator mAnim;
    #endregion
    private void Awake()
    {
        Setup();
    }
    private void OnEnable()
    {
        SetActions(true); 
    }
    private void OnDisable()
    {
        SetActions(false);
    }
    private void Update()
    {
        Movement();
        ClaimSystem();
        BalanceSystem();
        Victory();
        UpdateStickRotation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            Death();
        }
    }
    private void Setup()
    {
        GetScriptableObjects();
        mAnim.speed = 0;
    }
    private void SetActions(bool enabled)
    {
        if (enabled)
        {
            ObjectManager.MoveFinisherAction += CheckSticks;
            Gamemanager.PlayAction += Play;
        }
        else
        {
            ObjectManager.MoveFinisherAction -= CheckSticks;
            Gamemanager.PlayAction -= Play;
        }
    }
    private void Play()
    {
        skate.SetActive(true);
        mAnim.speed = 1;
        moveable = true;
    }
    private void GetScriptableObjects()
    {
        playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
        constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void ClaimSystem()
    {
        if (ObjectManager.Singleton.CurrentObject != null&&moveable)
        {
            if (!ObjectManager.Singleton.CurrentObject.IsUsed)
            {
                ClaimDirection();
            }
        }
    }
    private void ClaimObject(int direction)
    {
        int objectAmount = ObjectAmount(direction);
        if (objectAmount != 0)
        {
            if (direction == -1)
            {
                leftStick += objectAmount;
                ObjectManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, leftStickTransform);
            }
            else
            {
                rightStick += objectAmount;
                ObjectManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, rightStickTransform);
            }
            if (ObjectManager.Singleton.CurrentObject.ObjectCount < 0)
            {
                ObjectMoveToGround(objectAmount, direction);
            }
            UpdateLevelManager();
            UpdateStickText();
        }
    }
    private void UpdateLevelManager()
    {
        LevelManager.Singleton.UpdateStickObjectCount(leftStick, rightStick);
    }
    private int ObjectAmount(int direction)
    {
        int givenObject=0;
        if (ObjectManager.Singleton.CurrentObject.ObjectCount >0)
        {
            givenObject = ObjectManager.Singleton.CurrentObject.ObjectCount;
        }
        else
        {
            if (direction == -1)
            {
                if (Mathf.Abs(ObjectManager.Singleton.CurrentObject.ObjectCount) >= leftStick)
                {
                    givenObject = -leftStick;
                }
                else
                {
                    givenObject = (ObjectManager.Singleton.CurrentObject.ObjectCount + leftStick) - leftStick;
                }
            }
            else if (direction == 1)
            {
                if (Mathf.Abs(ObjectManager.Singleton.CurrentObject.ObjectCount)>=rightStick)
                {
                    givenObject = -rightStick;
                }
                else
                {
                    givenObject =(ObjectManager.Singleton.CurrentObject.ObjectCount + rightStick)-rightStick;
                }
            }
        }
        return givenObject;
    }
    private void Movement()
    {
        if (moveable)
        {
            transform.Translate(Vector3.forward * playerValues.PlayerSpeed*Time.deltaTime);
        }
    }
    private void ClaimDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseEndPos = Input.mousePosition;
            if (Mathf.Abs(_mouseStartPos.x - _mouseEndPos.x) > 120)
            {
                if (_mouseStartPos.x > _mouseEndPos.x)//Left
                {
                    ClaimObject(-1);
                }
                if (_mouseStartPos.x < _mouseEndPos.x)//Right
                {
                    ClaimObject(1);
                }
            }
        }
    }
    private void BalanceSystem()
    {
        if (!moveable)
        {
            return;
        }
        if (leftStick == rightStick)
        {
            balanceValue = Mathf.MoveTowards(balanceValue, 0.5f, constantVariables.BalanceChangeScale*0.5f* Time.deltaTime);
            mAnim.SetBool("leftFoot", false);
            mAnim.SetBool("rightFoot", false);
        }
        else
        {
            if (leftStick > rightStick)
            {
                balanceValue = Mathf.MoveTowards(balanceValue, 0, constantVariables.BalanceChangeScale * ((float)(leftStick-rightStick)/100) * Time.deltaTime);
                mAnim.SetBool("leftFoot", true);
                mAnim.SetBool("rightFoot", false);
            }
            else if (rightStick > leftStick)
            {
                balanceValue = Mathf.MoveTowards(balanceValue, 1, constantVariables.BalanceChangeScale * ((float)(rightStick-leftStick)/ 100) * Time.deltaTime);
                mAnim.SetBool("rightFoot", true);
                mAnim.SetBool("leftFoot", false);
            }
        }
        Overlay.Singleton.BalanceMeteer(balanceValue);
        PlayerBalance();
        if (balanceValue <= 0 || balanceValue >= 1)
        {
            Death();
        }
    }
    private void PlayerBalance()
    {
        transform.localEulerAngles = new Vector3(0, 0, -(balanceValue*180)+90);
    }
    private void CheckSticks()
    {
        leftStickObjects.Clear();
        rightStickObjects.Clear();
        Transform child = null;
        for (int i = 0; i < leftStickTransform.childCount; i++)
        {
            child = leftStickTransform.GetChild(i);
            child.DOLocalMoveY(0.05f * i, constantVariables.ObjectMoveDuration);
            leftStickObjects.Add(child);
        }
        for (int i = 0; i < rightStickTransform.childCount; i++)
        {
            child = rightStickTransform.GetChild(i);
            child.DOLocalMoveY(0.05f * i, constantVariables.ObjectMoveDuration);
            rightStickObjects.Add(child);
        }
    }
    private void UpdateStickText()
    {
        leftStickText.text = leftStick.ToString();
        rightStickText.text = rightStick.ToString();
    }
    private void UpdateStickRotation()
    {
        leftStickText.transform.localEulerAngles = new Vector3(0, 0, (balanceValue * 180)-90);
        rightStickText.transform.localEulerAngles = new Vector3(0, 0, (balanceValue * 180)-90);
    }
    private void ObjectMoveToGround(int count,int direction)
    {
        Transform child = null;
        List<Transform> removedList = new List<Transform>();
        leftStickObjects = leftStickObjects.OrderByDescending(x => x.position.y).ToList();
        rightStickObjects = rightStickObjects.OrderByDescending(x => x.position.y).ToList();
        for (int i = 0; i < Mathf.Abs(count); i++)
        {
            if (direction == -1)
            {
                child = leftStickObjects[i];
                child.GetComponent<StackedObjects>().MoveGroundAndReturnToPool(-1,constantVariables.ObjectMoveDelay*i);
            }
            else
            {
                child = rightStickObjects[i];
                child.GetComponent<StackedObjects>().MoveGroundAndReturnToPool(1, constantVariables.ObjectMoveDelay * i);
            }
            removedList.Add(child);
        }
        for (int i = 0; i < removedList.Count; i++)
        {
            leftStickObjects.Remove(removedList[i]);
        }
    }
    private void Death()
    {
        if (ObjectManager.Singleton.CurrentObject != null)
        {
            if (!ObjectManager.Singleton.CurrentObject.IsUsed)
            {
                moveable = false;
                mAnim.enabled = false;
                stickBody.constraints = RigidbodyConstraints.None;
                stickBody.isKinematic = false;
                leftStickText.gameObject.SetActive(false);
                rightStickText.gameObject.SetActive(false);
                for (int i = 0; i < leftStickObjects.Count; i++)
                {
                    leftStickObjects[i].SetParent(null);
                    leftStickObjects[i].GetComponent<StackedObjects>().EnableComponents();
                }
                for (int i = 0; i < rightStickObjects.Count; i++)
                {
                    rightStickObjects[i].SetParent(null);
                    rightStickObjects[i].GetComponent<StackedObjects>().EnableComponents();
                }
                Gamemanager.Singleton.Failed();
            }
        }
    }
    private void Victory()
    {
        if (moveable&&!victory)
        {
            if (transform.position.z > LevelManager.Singleton.FinishLine.position.z)
            {
                victory = true;
                mAnim.SetBool("leftFoot", false);
                mAnim.SetBool("rightFoot", false);
                mAnim.SetBool("idle", true);
                skate.SetActive(false);
                transform.DOLocalRotate(Vector3.zero, 1);
                DOVirtual.DelayedCall(0.5f, () =>
                 {
                     moveable = false;
                     Gamemanager.Singleton.Victory();
                 });
            }
        }
    }
}
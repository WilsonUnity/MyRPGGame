/*
 * ActionControl：角色行为控制类
 * 脚本挂载对象：Player(角色控制柄)
 * 作者：林逸群
 * 日期：2018
 * 修改记录：10/xx -使用事件驱动设计,优化与StateMachine脚本的通信
 *          11/1 -代码可读性优化
 *          11/5 -控制器实现无缝切换
 * 主题：
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using IUserInterface;
using UnityEngine;

public class ActionControl : MonoBehaviour
{
    #region Public_Variable
    
    //角色控制柄挂载的动画组件
    public Animator _animator;
    
    //人物模型
    public GameObject playerModel;

    //相机控制类实例
    public CameraControl camcon;
    
    //是否锁住当前的移动
    public bool isLockmoving = false;
    
    //走路（散步）速度
    public float walkSpeed = 2;
    
    //奔跑速度
    public float runSpeed = 2;
    
    //跳跃高度，变量_jumpSpeed的y轴分量
    public float jumpHeight = 3;
    
    //前滚速度
    public float rollForwardSpeed = 0.0f;
    
    //后滚速度
    public Vector3 rollbackSpeed;
    
    //是否左手持盾
    public bool leftIsShield = true;
    
    [Header("===== Friction Setting =====")]
    //物理材质摩擦0
    public PhysicMaterial frictionzero;
    //物理材质摩擦1
    public PhysicMaterial frictionone;
    
    #endregion
    
    //-----------------------------------------------------------
    
    #region Private_Variable   
    
    //移动速度
    private Vector3 _movingVec;
    
    //动画权重的当前值
    private float _currentValue;
    
    //跳跃速度
    private Vector3 _jumpSpeed;
    
    //是否可以攻击
    private bool IsAttack;

    //动画自身的移动量，默认状态下不使用动画默认的移动量
    private Vector3 _deltaPos;
    
    //获取角色控制柄的碰撞体
    private CapsuleCollider _col; 
    
    //角色控制柄挂载的刚体
    private Rigidbody _rigidbodyrig;
    
    //当前是否打开背包界面
    private bool isOpenBGUIForm = false;
    
    #endregion
    private IInputState _inputState;
    public IInputState InputState
    {
        set { _inputState = value; }
        get { return _inputState; }
    }
    
    //-----------------------------------------------------------
    
    #region Delegate
    
    public delegate void OnActionEvent();
    public event OnActionEvent OnACtion; 
    
    #endregion  
    
    //-----------------------------------------------------------

    private void Awake()
    {
        //获取人物模型
        playerModel = this.transform.Find("ybot").gameObject;
        //获取角色控制柄下的刚体组件
        _rigidbodyrig = this.GetComponent<Rigidbody>();
        //获取角色控制柄下的碰撞体组件
        _col = this.transform.GetComponent<CapsuleCollider>();
        
        //如果模型存在，则获取模型下的动画状态机
        if (playerModel)
        {
            _animator = playerModel.GetComponent<Animator>();
        }

    }
    
    private void Start()
    {
        //事件注册，与StateMachine脚本的通信
        //只有角色本身才可以注册状态机事件，所以必须区分开
        if (this.transform.name == "Player")
        {
            EventManager.Instance.AddListener(EventManager.EVENT_TYPE.FSM_ENTER, EnterEvent);
            EventManager.Instance.AddListener(EventManager.EVENT_TYPE.FSM_EXIT, ExitEvent);
            EventManager.Instance.AddListener(EventManager.EVENT_TYPE.FSM_UPDATE, UpdateEvent);
        }
    }
    
    //-----------------------------------------------------------

    #region Event

    /// <summary>
    /// StateMachine脚本触发Enter时调用
    /// </summary>
    /// <param name="Event_Type">事件类型</param>
    /// <param name="Sender">发送者对象</param>
    /// <param name="Param">参数，可选</param>
    public void EnterEvent(EventManager.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //发出对象为目前操作的角色
        if(Sender.name.Equals("Player"))
            switch ((string) Param)
            {
                case "Jump":
                    OnjumpEnter();
                    break;
                case "Ground":
                    OnGroundEnter();
                    break;
                case "Jab":
                    OnJabEnter();
                    break;
                case "General":
                    OnGeneralEnter();
                    break;
                default:
                    Debug.Log("EnterEvent Error");
                    break;
            }
    }

    /// <summary>
    /// StateMachine脚本触发Exit时调用
    /// </summary>
    /// <param name="Event_Type">事件类型</param>
    /// <param name="Sender">发送者对象</param>
    /// <param name="Param">参数，可选</param>
    public void ExitEvent(EventManager.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if(Sender.name.Equals("Player"))
            switch ((string) Param)
            {
                case "Ground":
                    OnGroundExit();
                    break;
                case "Roll":
                    RollExit();
                    break;
                case "Jab":
                    OnJabExit();
                    break;
                default:
                    Debug.Log("ExitEvent Error");
                    break;
            }
    }

    /// <summary>
    /// StateMachine脚本触发Update时调用
    /// </summary>
    /// <param name="Event_Type">事件类型</param>
    /// <param name="Sender">发送者对象</param>
    /// <param name="Param">参数，可选</param>
    public void UpdateEvent(EventManager.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if(Sender.name.Equals("Player"))
            switch ((string) Param)
            {
                case "Roll":
                    OnRollUpdate();
                    break;
                case "Jab":
                    OnJabUpdate();
                    break;
                default:
                    Debug.Log("FSM Error");
                    break;
            }
    }

    #endregion
    
    //-----------------------------------------------------------
    
    private void Update()
    {
        if (_animator)
        {
            //相机没有锁住物体时
            if (camcon.LockState == false)
            {
                //根据Dmag值来调节混合动画，线性插值可以防止走路跟奔跑的动作过渡太过突然
                _animator.SetFloat("forward",
                    _inputState.DMag * Mathf.Lerp(_animator.GetFloat("forward"), (_inputState.isrun ? 2.0f : 1.0f), 0.1f));
                _animator.SetFloat("Right", 0);
            }
            else //相机锁住物体时，即聚焦模式，移动动作变更
            {
                //将旋转量从世界空间转变到局部空间
                Vector3 localDvec = transform.InverseTransformVector(_inputState.Dvec);
                _animator.SetFloat("forward", localDvec.z);
                _animator.SetFloat("Right", localDvec.x);
            }
            
            //触发锁定
            if (_inputState.islock)
            {
                camcon.LockUnLock();
            }

            if (_rigidbodyrig.velocity.magnitude > 1)
            {
                _animator.SetTrigger("Roll");
            }

            //触发跳跃
            if (_inputState.isjump)
            {
                _animator.SetTrigger("jump");
                IsAttack = false;
            }

            //轻攻击操作
            //rb : 右手攻击 ，lb ：左手攻击
            if ((_inputState.rb || _inputState.lb) &&
                (CheckState("Ground") ||
                 CheckStateTag("attackL") ||
                 CheckStateTag("attackR")) && IsAttack)
            {
                if (_inputState.rb)
                {
                    _animator.SetBool("mirror", false);
                    _animator.SetTrigger("attack");
                }

                //左手持剑下也可以攻击
                else if (_inputState.lb && !leftIsShield)
                {
                    _animator.SetBool("mirror", true);
                    _animator.SetTrigger("attack");
                }


            }

            //重攻击操作
            if ((_inputState.rt || _inputState.lt) &&
                (CheckState("Ground") ||
                 CheckStateTag("attackL") ||
                 CheckStateTag("attackR")) && IsAttack)
            {
                if (_inputState.rt)
                {
                    print("do heavy attack");
                }
                //左手操作，如果持剑，进行重攻击。如果持盾，进行盾反
                else
                {
                    if (!leftIsShield)
                    {
                        print("do heavy attack");
                    }
                    else
                    {
                        _animator.SetTrigger("counterback");
                    }
                }
            }

            //特殊攻击
            if (_inputState.isFontStab)
            {
                OnACtion.Invoke();
            }

            //装备盾牌的情况下
            if (leftIsShield)
            {
                float CurrentWeight = 0;
                //只有处于Ground动画状态才能举盾
                if (CheckState("Ground"))
                {
                    _animator.SetBool("Defense", _inputState.isdefense);
                    if (_inputState.isdefense == true)
                    {
                        CurrentWeight = 1;
                        _animator.SetLayerWeight(_animator.GetLayerIndex("Defense"),
                            Mathf.SmoothDamp(_animator.GetLayerWeight(_animator.GetLayerIndex("Defense")), CurrentWeight,
                                ref _currentValue, 0.1f));
                    }
                    else
                    {
                        _animator.SetLayerWeight(_animator.GetLayerIndex("Defense"),
                            Mathf.SmoothDamp(_animator.GetLayerWeight(_animator.GetLayerIndex("Defense")), 0, ref _currentValue,
                                0.1f));
                    }
                } //非Ground动画状态强制取消举盾动作
                else
                {
                    _animator.SetBool("Defense", false);
                    _animator.SetLayerWeight(_animator.GetLayerIndex("Defense"),
                        Mathf.SmoothDamp(_animator.GetLayerWeight(_animator.GetLayerIndex("Defense")), 0, ref _currentValue, 0.1f));
                }
            }
            //非举盾状态下将Ddfense层的权重拉到0
            else
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex("Defense"),
                    Mathf.SmoothDamp(_animator.GetLayerWeight(_animator.GetLayerIndex("Defense")), 0, ref _currentValue, 0.1f));
            }


            //未锁定某一对象
            if (camcon.LockState == false)
            {
                //只有摇杆值不为0时才进行角色方向变更
                if (_inputState.DMag > 0.1f)
                {
                    //利用线性插值使角色旋转变得平滑
                    playerModel.transform.forward =
                        Vector3.Slerp(playerModel.transform.forward, _inputState.Dvec, 0.3f);
                }

                //移动速度的计算
                if (!isLockmoving) 
                {
                    _movingVec = _inputState.DMag * playerModel.transform.forward * walkSpeed *
                                (_inputState.isrun ? runSpeed : 1.0f);
                }
            }
            
            else //处于锁定状态
            {
                //模型指向角色控制柄
                playerModel.transform.forward = transform.forward;
                if (!isLockmoving)
                {
                    //由于锁定状态下forward被锁死，所以要根据旋转Dvec来调整移动
                    _movingVec = _inputState.Dvec * walkSpeed;
                }
            }
            
        }//if(_animator)_End

        if (_inputState.isStart && !isOpenBGUIForm)
        {
            UIFormsMgr.GetInstance().ShowUIForm("BG_UI");
            Cursor.lockState = CursorLockMode.None;
            isOpenBGUIForm = true;
        }
        else
        if(_inputState.isStart && isOpenBGUIForm)
        {
            UIFormsMgr.GetInstance().CloseUIForm("BG_UI");
            Cursor.lockState = CursorLockMode.Locked; 
            isOpenBGUIForm = false;
        }

    }//Update_End
    
    private void FixedUpdate()
    {
        //刚体组件不为空
        if (_rigidbodyrig)
        {
            //当前位置加上动画自身移动量
            _rigidbodyrig.position += _deltaPos;
            //运动公式
            _rigidbodyrig.velocity = new Vector3(_movingVec.x, _rigidbodyrig.velocity.y, _movingVec.z) + _jumpSpeed +
                           (rollForwardSpeed * playerModel.transform.forward) +
                           rollbackSpeed;
            _jumpSpeed = Vector3.zero;
            _deltaPos = Vector3.zero;
        }
    }

     
    //-----------------------------------------------------------
    
    #region 私有方法

    //常规动画Enter处理
    private void OnGeneralEnter()
    {
        _inputState.IsInputEnable = false;
        _movingVec = Vector3.zero;
    }

    //跳跃动画Enter处理
    private void OnjumpEnter()
    {
        //跳跃间将摇杆禁用
        _inputState.IsInputEnable = false;
        
        //设置y轴分量
        _jumpSpeed = new Vector3(0, jumpHeight, 0);
        
        //将移动冲量锁死不受遥感禁用的影响，不然会原地跳跃
        isLockmoving = true;
    }

    //Ground动画Enter处理
    private void OnGroundEnter()
    {
        //摇杆启用
        _inputState.IsInputEnable = true;
        //不锁住速度
        isLockmoving = false;
        //允许攻击
        IsAttack = true;
        //物理材质替换，摩檫力此时为（1，1，1）
        _col.material = frictionone;
    }
    
    //后滚动画Enter处理
    private void OnJabEnter()
    {
        _inputState.IsInputEnable = false;
        isLockmoving = true;
    }
    
    //Ground动画Exit处理
    private void OnGroundExit()
    {
        //摩檫力设置为（0，0，0）
        _col.material = frictionzero;
    }

    //前翻滚动画Exit处理
    private void RollExit()
    {
        //翻滚速度为0
        rollForwardSpeed = 0.0f;
        //启用摇杆
        _inputState.IsInputEnable = true;
        // _movingVec = Vector3.zero;
    }
    
    //后翻滚动画Exit处理
    private void OnJabExit()
    {
        _inputState.IsInputEnable = true;
        isLockmoving = false;
    }

    //前翻滚动画Update处理
    private void OnRollUpdate()
    {
        
        rollForwardSpeed = 4;
        _inputState.IsInputEnable = false;
        //此时移动速度归0，不然翻滚距离会很远
        _movingVec = Vector3.zero;
    }

    //后滚动画Update处理
    //字符串JabVelocity是状态机的浮点数变量，与Jab动画曲线绑定
    private void OnJabUpdate()
    {
        rollbackSpeed = playerModel.transform.forward * (_animator.GetFloat("JabVelocity"));
    }
    
    //使用动画自身的运动
    private void OnUpdateRm(Vector3 _delta)
    {
        if(CheckState("SlashC") || CheckState("SlashC 0") || CheckState("SlashA") || CheckState("SlashA 0"))
        {
            _deltaPos += (_deltaPos + _delta) * 0.4f;
        }
    }

    #endregion

    #region 待优化

    //角色胶囊体与地面碰撞时调用
    public void IsGround()
    {
        _animator.SetBool("IsGround", true);
    }

    //角色胶囊体不与地面碰撞时调用
    public void NotGround()
    {
        _animator.SetBool("IsGround", false);
    }
    
    /// <summary>
    /// 是否触发动画
    /// </summary>
    /// <param name="triggerName"></param>
    public void IssueTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
    
    public void SetBool(string StateName,bool value)
    {
        _animator.SetBool(StateName, value);
    }
    
    /// <summary>
    /// 检查当前是否处于所指定的动画状态，以状态名为检索依据
    /// </summary>
    /// <param name="StateName">状态名</param>
    /// <param name="LayerName">动画层</param>
    /// <returns></returns>
    public bool CheckState(string StateName, string LayerName = "Base Layer")
    {
        int Layerindex = _animator.GetLayerIndex(LayerName);
        return _animator.GetCurrentAnimatorStateInfo(Layerindex).IsName(StateName);
    }
	
    /// <summary>
    /// 检查当前是否处于所指定的动画状态，以标签名（tag）为检索依据
    /// </summary>
    /// <param name="tagName">动画状态tag</param>
    /// <param name="LayerName">动画层</param>
    /// <returns></returns>
    public bool CheckStateTag(string tagName, string LayerName = "Base Layer")
    {
        int Layerindex = _animator.GetLayerIndex(LayerName);
        return _animator.GetCurrentAnimatorStateInfo(Layerindex).IsTag(tagName);
    }
    
   

    #endregion
    
    #region 旧代码，用于参考，可直接删除

    //----------------------------------------abandon data for reference only---------------------------------------------- 
//    public void OnAttackUpdate()
//    {
//        //rollbackSpeed = playerModel.transform.forward * (anim.GetFloat("Attack1Velocity"));
//    }

//    public void OnHitEnter()
//    {
//        userInput.IsInputEnable = false;
//        _movingVec = Vector3.zero;
//    }
//
//    public void CounterBackEnter()
//    {
//        userInput.IsInputEnable = false;
//        _movingVec = Vector3.zero;
//    }
//
//    public void StunnedEnter()
//    {
//        userInput.IsInputEnable = false;
//        _movingVec = Vector3.zero;
//    }
//
//
//    public void OnLockEnter()
//    {
//        userInput.IsInputEnable = false;
//        _movingVec = Vector3.zero;
//    }

//    public void OnAttackEnter()
//    {
//        userInput.IsInputEnable = false;
//
//    }
    
    //    public void FallState()
//    {
//        userInput.IsInputEnable = false;
//        isLockmoving = true;
//    }
//------------------------------------------------------------------------------------------------

    #endregion

}

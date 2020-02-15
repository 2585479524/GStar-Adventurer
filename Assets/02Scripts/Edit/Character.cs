using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using DG.Tweening;
/// <summary>
/// 处理主角各中操作输入
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Character : Role
{
    public enum EatDir
    {
        Up,
        Front,
        Down
    }
    #region 变量
    public float ShootSpeed = 6f;
    // private bool[] HaveBlock = new bool[3] { false, false, false };
    //private bool CanMove;
    public float Speed = 3f;
    // private Chunk pointChunk;

    public float JumpSpeed = 6f;
    //private float Timer = 1f;
    //private bool IsTouch = false;

    private Touch touch;

    //private float TouchRange;
    // private Vector3 LastDir;
    //private bool HaveTouchNoUi;
    private Vector3 RayOffset = new Vector3(0, -0.3f, 0);
    //private bool CanDeteJump = true;

    //
    private BlockType GetBlock;
    private GameObject GetObject;

    private bool IsAttack;
    private float AttackTime = 0.5f;
    //吃按钮是否按下
    private Vector3 AttackPos;
    public Transform GiozmsCube;
    public Material CubeMaterial;
    public Material MainMaterial;
    private bool eatbutton;
    private bool IsEatStart
    {
        set
        {
            eatbutton = value;

        }
        get
        {
            return eatbutton;
        }
    }
    //前方坐标
    private Vector3 MousePos;
    //选中的坐标
    private Vector3 CurrentPos;
    //选中的方块类型
    private BlockType CurrentBlockType;
    //选中的位置是否为空
    private bool BlockIsNull;
    //按钮状态是吃还是放，通过IsEat访问
    private bool Eat = true;
    /* private GameObject hitObject;//检测到的物体
    
     private GameObject EatObject;*/
    private bool IsTest = false;
    private bool IsReadyToShoot = false;
    private bool IsEatButton;
    private bool IsShoot = false;
    private bool EatACube;
    private bool StopOp = false;
    //方向修正
    public Transform Camera;
    private float CameraRotaAngle;

    private GameObject Prefab;
    private EatDir Dir = EatDir.Front;


    private Animator Animator;
    /// <summary>
    /// 吃东西的时间
    /// </summary>
    public float EatBlockTime = 1f;
    private float TimeEat = 1f;

    /// <summary>
    /// 吐的冷却
    /// </summary>
    private float ShootTime = 1f;
    private bool CanShoot = true;

    public GameObject Par;
    public bool StartWin;

    private AudioSource stepAudioSource;
    #endregion


    #region 事件发送
    public bool IsEat
    {
        set
        {
            Eat = value;
            MessageManager.Instance.DeOrAdd(!Eat);
        }
        get
        {
            return Eat;
        }
    }
    private float EatTime
    {
        set
        {
            TimeEat = value;
            MessageManager.Instance.EatTimer(EatTime, EatBlockTime);
        }
        get
        {
            return TimeEat;
        }
    }
    private EatDir PlayerEatDir
    {
        set
        {
            //状态改变
            if (value != Dir)
            {
                Dir = value;
                EatTime = EatBlockTime;
            }
            Vector3 Cur = new Vector3();
            switch (value)
            {
                case EatDir.Up:
                    Cur = MousePos + Vector3.up;
                    break;
                case EatDir.Front:
                    Cur = MousePos;
                    break;
                case EatDir.Down:
                    Cur = MousePos + Vector3.down;
                    break;
            }
            if (IsEatButton || IsReadyToShoot)
            {
                CurrentPos = Cur;
            }
            if (IsEat)
            {
                if (!Chunk.GetBlockIsShow(CurrentPos))
                {
                    Debug.Log("该方块已隐藏");
                    CurrentPos = MousePos;
                }
            }
            CurrentBlockType = Chunk.GetBlock(CurrentPos);
        }
        get
        {
            return Dir;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody = GetComponent<Rigidbody>();

        Prefab = Resources.Load("Prefabs/Cube") as GameObject;

        Rigidbody = GetComponent<Rigidbody>();

        stepAudioSource = GetComponent<AudioSource>();

        Animator = transform.GetChild(0).GetComponent<Animator>();
        //GiozmsCube = transform.GetChild(1);
        //material = GiozmsCube.GetComponent<MeshRenderer>().material;
        MainMaterial.color = Color.white;
        //material.renderQueue += 1;
        //material.renderQueue += 2000;
    }
    public void Init()
    {
        m_hp = MaxHp;
        MessageManager.Instance.HpChange(MaxHp);
    }
    #region 事件监听
    private void Awake()
    {
        MessageManager.Instance.EatButtonUp += EatButtonUp;
        MessageManager.Instance.SelectedItem += SelectedItem;
        MessageManager.Instance.Shoot += ShootCube;
        MessageManager.Instance.MainCharShowWin += ShowVictory;
    }

    private void ShowVictory()
    {
        MessageManager.Instance.InstanceParticle(ParticleType.Success, transform.position,null);
        StopOp = true;
        StartWin = true;
        Animator.SetTrigger("victory");
    }

    /// <summary>
    /// 停止吃
    /// </summary>
    private void EatStop()
    {
        IsTest = false;
        IsEatStart = false;
        Animator.SetBool("Eat", false);
        EatTime = 1f;
    }
    private void SelectedItem(BlockType obj)
    {
        GetBlock = obj;
        if (obj == BlockType.None)
        {
            IsEat = true;
        }
        else
        {
            IsEat = false;
        }
    }
    #endregion
    // Update is called once per frame

    private void FixedUpdate()
    {
        if (StopOp)
        {
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
            if (StartWin)
            {
                transform.forward = Vector3.Lerp(transform.forward, -Camera.forward, Time.deltaTime * 10f);
            }
            return;
        }
        if (IsTrap || IsAttack)
        {
            AttackTime -= Time.deltaTime;
            if (AttackTime <= 0)
            {
                AttackTime = 0.5f;
                //IsAttack = false;
                IsTrap = false;
                IsAttack = false;
            }
            Vector3 dir = transform.position - AttackPos;
            dir = dir.normalized;
            dir *= 4f;
            dir.y = 6f;
            Rigidbody.velocity = dir;
            //return;
        }
        else
        {
            AttackTime = 0;
        }
        MobileInputMove();
        //DeteFront();
        if (CrossPlatformInputManager.GetButtonDown("Eat"))
        {
            EatACube = false;
        }
        if (CrossPlatformInputManager.GetButton("Eat"))
        {
            IsEatButton = true;
            IsEatStart = true;
            IsTest = true;
        }
        if (IsTest)
        {
            TestFront();
        }

        if (IsEatStart && IsEat && CurrentBlockType != BlockType.Bedrock)
        {
            EatTest();
        }

        if (IsReadyToShoot)
        {
            ShootTest();
        }
        //射击冷却
        if (!CanShoot)
        {
            ShootTime -= Time.deltaTime;
            MessageManager.Instance.ShootCold(1f - ShootTime);
            if (ShootTime <= 0)
            {
                ShootTime = 1f;
                MessageManager.Instance.ShootCold(1);
                CanShoot = true;
            }
        }
        DrawGizmos();
    }
    protected override void Update()
    {
        if (StopOp)
        {
            return;
        }
        base.Update();

    }

    /// <summary>
    /// 吃的尝试
    /// </summary>
    private void EatTest()
    {
        //TestFront();
        ///吃的逻辑=
        if (!BlockIsNull)
        {
            EatTime -= Time.deltaTime;
            Animator.SetBool("Eat", true);
            if (EatTime <= 0)
            {
                EatTime = EatBlockTime;
                //GetBlock = CurrentBlockType;
                Chunk chunk = Chunk.GetChunk(CurrentPos);
                if (chunk)
                {
                    SoundManager.Instance.PlaySfx("PlayerEat", transform.position);
                   
                    chunk.ChangeBlock(CurrentPos, BlockType.None);
                    if (CurrentBlockType == BlockType.LoveCube)
                    {
                        HP++;
                        MessageManager.Instance.HpChange(HP);
                        MessageManager.Instance.InstanceParticle(ParticleType.EatHPCube, transform.position,null);
                    }
                    else
                    {
                        MessageManager.Instance.AddBagItem(CurrentBlockType, 1);
                    }
                    MessageManager.Instance.FindBlock(CurrentBlockType);
                    //IsEat = false;
                    EatACube = true;
                    EatStop();
                }
            }
        }
        else
        {
            EatStop();
        }

    }
    bool IsCloseAxis(Vector3 Dir)
    {
        if (Vector3.Angle(Dir, transform.forward) <= 45f)
        {
            transform.forward = Dir;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 射击尝试
    /// </summary>
    private void ShootTest()
    {
        //先修正朝向
        //四个方向计算角度
        if (IsCloseAxis(Vector3.forward)) { }
        else if (IsCloseAxis(Vector3.right)) { }
        else if (IsCloseAxis(Vector3.left)) { }
        else if (IsCloseAxis(Vector3.back)) { }
        TestFront();
    }
    /// <summary>
    /// 用于检测前方的位置
    /// </summary>
    private void TestFront()
    {
        //修正当前的坐标
        Vector3 TestDir = new Vector3();
        Ray ray = new Ray(transform.position + RayOffset, transform.forward);
        RaycastHit hitInfo;
        LayerMask layerMask = 1 << 10;

        if (Physics.Raycast(ray, out hitInfo, 1f, layerMask.value))
        {
            TestDir = hitInfo.point + ray.direction * 0.01f;
        }
        else
        {
            TestDir = transform.position + RayOffset + transform.forward;
        }
        TestDir = FromWorldPositionToCubePosition(TestDir);
        //
        if (TestDir != MousePos)
        {
            MousePos = TestDir;
            ChangeDir();
        }
        float dirValue = CrossPlatformInputManager.GetAxis("Vertical_Eat");
        if (dirValue > 0.33f)
        {
            PlayerEatDir = EatDir.Up;
        }
        else if (dirValue > -0.33f)
        {
            PlayerEatDir = EatDir.Front;
        }
        else
        {
            PlayerEatDir = EatDir.Down;
        }
        switch (CurrentBlockType)
        {
            case BlockType.None:
                BlockIsNull = true;
                break;
            case BlockType.Object:
                BlockIsNull = true;
                break;
            default:
                BlockIsNull = false;
                break;
        }
    }
    /// <summary>
    /// 改变了方向
    /// </summary>
    private void ChangeDir()
    {
        if (IsEat && IsTest)
        {
            EatStop();
        }
    }

    public void EatButtonUp()
    {
        if (StopOp)
        {
            return;
        }
        IsEatButton = false;
        if (IsEat)
        {

        }
        else
        {
            IsTest = false;
            if (!EatACube)
            {
                if (BlockIsNull)
                {
                    SoundManager.Instance.PlaySfx("PlayerPlaceBlock", transform.position);
                    //放操作
                    Chunk chunk = Chunk.GetChunk(CurrentPos);
                    if (chunk)
                    {
                        chunk.ChangeBlock(CurrentPos, GetBlock);
                        MessageManager.Instance.DeleteBagItem(GetBlock, 1);
                        MessageManager.Instance.InstanceParticle(ParticleType.PutCube, CurrentPos,null);
                        //IsEat = true;
                    }
                }
                EatStop();
            }
        }
    }

    /// <summary>
    ///射出方块
    /// </summary>
    public void ShootCube()
    {
        if (StopOp)
        {
            return;
        }
        if (!CanShoot)
        {
            return;
        }
        IsTest = false;
        IsReadyToShoot = false;
        if (!BlockIsNull)
        {
            return;
        }
        CanShoot = false;
        Animator.SetTrigger("spit");
        IsShoot = true;
        Invoke("Instante", 0.46f);
        //取对象
    }
    public void Instante()
    {
        SoundManager.Instance.PlaySfx("PlayerSpit", transform.position);
      
        GameObject cube = ObjectPool.Instance.Spawn("Cube");
        if (cube)
        {
            //Vector3 pos = transform.position + transform.forward * 1.8f;
            //pos.y = CurrentPos.y;
            cube.GetComponent<CubeInstance>().InstanceFly(GetBlock, transform.forward, ShootSpeed, CurrentPos);
            MessageManager.Instance.DeleteBagItem(GetBlock, 1);
        }
        MessageManager.Instance.InstanceParticle(ParticleType.StarTrail, MousePos, cube.transform);
        Invoke("ShootStop", 0.5f);
    }
    public void ShootStop()
    {
        IsShoot = false;
    }
    public void ReadyToShoot()
    {
        IsReadyToShoot = true;
        IsTest = true;
    }

    void MobileInputMove()
    {
        if ((CrossPlatformInputManager.GetButton("Jump") || Input.GetKey(KeyCode.Space)) && CanJump)
        {
            Par.SetActive(false);
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpSpeed, Rigidbody.velocity.z);
            //Rigidbody.AddForce(Vector3.up * mJumpSpeed);
            CanJump = false;
            Animator.SetTrigger("Jump");
            SoundManager.Instance.PlaySfx("PlayerJump", transform.position);
        }
        //PC端移动输入
        if (IsShoot)
        {
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float h2 = CrossPlatformInputManager.GetAxis("Horizontal");
        float v2 = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 dir = new Vector3(h + h2, 0, v + v2);
        //输入修正
        dir = Quaternion.Euler(0, Camera.localEulerAngles.y, 0) * dir;
        dir = dir.normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            SoundManager.Instance.PlayStepSfx(stepAudioSource, "PlayerStep");
            if (IsReadyToShoot)
            {
                transform.forward = dir;
                Vector3 v1 = transform.forward * dir.sqrMagnitude * Speed;
                Rigidbody.velocity = new Vector3(v1.x, Rigidbody.velocity.y, v1.z);
                Animator.SetFloat("Speed", v1.sqrMagnitude);
            }
            else
            {
                float angle = Vector3.Angle(transform.forward, dir);

                transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * (angle / 36f + 15));

                if (angle < 45)
                {
                    Vector3 v1 = dir * dir.sqrMagnitude * Speed;
                    Rigidbody.velocity = new Vector3(v1.x, Rigidbody.velocity.y, v1.z);
                    Animator.SetFloat("Speed", v1.sqrMagnitude);
                }
            }

        }
        else
        {
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
            Animator.SetFloat("Speed", 0);
            //transform.forward = LastDir;
        }

    }

    private void TouchControl()
    {
        //移动端移动输入
        MobileInputMove();
    }
    //方块辅助 
    private void DrawGizmos()
    {
        if (!IsTest && !IsEatButton && !IsReadyToShoot)
        {
            CubeMaterial.color = new Color(0, 0, 0, 0);
            return;
        }

        if (IsEat)
        {
            if (BlockIsNull || CurrentBlockType == BlockType.Bedrock)
            {
                CubeMaterial.color = Color.red;
            }
            else
            {
                CubeMaterial.color = Color.white;
            }
        }
        else
        {
            if (!BlockIsNull)
            {
                CubeMaterial.color = Color.red;
            }
            else
            {

                CubeMaterial.color = Color.white;
            }
        }
        GiozmsCube.position = CurrentPos;
    }

    /// <summary>
    /// 把坐标化为标准的方块坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3 FromWorldPositionToCubePosition(Vector3 position)
    {
        Vector3 resut = Vector3.zero;
        resut.x = position.x > 0 ? (int)position.x * 1f + 0.5f : (int)position.x * 1f - 0.5f;
        resut.y = position.y > 0 ? (int)position.y * 1f + 0.5f : (int)position.y * 1f - 0.5f;
        resut.z = position.z > 0 ? (int)position.z * 1f + 0.5f : (int)position.z * 1f - 0.5f;
        return resut;
    }
    private void OutMapBack()
    {
        StopOp = false;
    }
    private void AttackBack()
    {
        MainMaterial.color = new Color(1, 1, 1, 1);
    }
    #region 回调

    public override void Attack(AttackType attackType, int Damage, Vector3 pos)
    {
        Debug.Log("主角受到类型伤害:" + attackType + "，掉血:" + Damage);
        HP -= Damage;
        MessageManager.Instance.HpChange(HP);
        SoundManager.Instance.PlaySfx("PlayerHurted", transform.position);
        if (attackType != AttackType.High)
        {
            if (attackType == AttackType.OutMap)
            {
                Rigidbody.velocity = Vector3.zero;
                Ishigh = false;
                transform.position = LastPos;
                StopOp = true;
                Invoke("OutMapBack", 0.4f);
            }
            else
            {
                IsAttack = true;
                AttackPos = pos;
            }
        }
        MainMaterial.color = new Color(1, 0.5f, 0.5f, 1);
        Invoke("AttackBack", 0.3f);
    }
    public override void ToGround()
    {
        //GameManager.Instance.Sound.PlayEffect("PlayerToGround");
        if (Ishigh)
        {
            Attack(AttackType.High, 1, Vector3.zero);
            Ishigh = false;
        }
        SoundManager.Instance.PlaySfx("PlayerToGround", transform.position);
        Par.SetActive(true);
        MessageManager.Instance.InstanceParticle(ParticleType.Jump, transform.position,null);
    }
    public override void Death()
    {
        if (StopOp)
        {
            return;
        }
        SoundManager.Instance.PlaySfx("PlayerDie", transform.position);
        StopOp = true;
        Animator.SetTrigger("death");
        MessageManager.Instance.RoleDeath();
    }
    #endregion
}

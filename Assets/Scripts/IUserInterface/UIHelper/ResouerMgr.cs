/*
 * IUserInterface : 用户界面系统
 * ResouerMgr ： 在Resourse.Load基础上增加了缓存的功能
 * 日期 ：2018
 * 程序员 : 林逸群
 * 注 ： 存在打印代码，游戏打包前请删除（49,84,87）
 */
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

namespace IUserInterface
{
    public class ResouerMgr : MonoBehaviour
    {
        //本脚本实例
        private static ResouerMgr _instance;

        //由于无法确定资源的具体类型，所以使用哈希表
        private Hashtable hb = null;
        
        //返回本脚本实例
        public static ResouerMgr GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameObject("_Resourse").AddComponent<ResouerMgr>();
            }

            return _instance;
        }

        private void Awake()
        {
            //初始化哈希表
            hb = new Hashtable();
        }

        
       /// <summary>
       /// 在Resourse.load的基础上加入了缓存的功能
       /// </summary>
       /// <param name="path">路径名称</param>
       /// <param name="isChace">是否进行缓存</param>
       /// <typeparam name="T"></typeparam>
       /// <returns></returns>
        private T LoadResourse<T>(string path, bool isCache) where T : UnityEngine.Object
        {
            //开头先检查该路径资源是否已经缓存过，如果是直接返回
            if (hb[path] != null)
            {
                return hb[path] as T;
            }

            T TResourseAsset = Resources.Load<T>(path);
            
            if (TResourseAsset == null)
            {
                Debug.LogError("资源提取失败，请检查原因");
            }
            else
            if(isCache) //是否执行缓存
            {
                hb.Add(path, TResourseAsset);
            }

            return TResourseAsset;
        }
        

        /// <summary>
        /// 当调用此方法时已经完成了资源的提取工作，此时将资源实例化到场景
        /// 如果不想启用缓存功能，可以将isCache设置为false
        /// 启用缓存可以提高性能，但是会增加内存负担。所以，请合理使用缓存功能
        /// </summary>
        /// <param name="path">路径名称</param>
        /// <param name="isCache">是否启用缓存</param>
        /// <returns></returns>
        public GameObject LoadAsset(string path, bool isCache)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("资源路径名称错误: " + path);
            }

            //根据路径提取并实例化到场景
            GameObject goClone = GameObject.Instantiate(LoadResourse<GameObject>(path, isCache));
            
            if (goClone == null)
            {
                Debug.LogError("将物件实例化到场景中失败");
            }

            return goClone;
        }


    }
}


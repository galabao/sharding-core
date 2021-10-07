﻿using System;
using System.Collections.Generic;
using System.Text;
using ShardingCore.Sharding.Abstractions;
using ShardingCore.Sharding.ReadWriteConfigurations.Abstractions;

namespace ShardingCore.Extensions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/10/5 8:37:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class ShardingReadWriteExtension
    {
        /// <summary>
        /// 设置读写分离读取写数据库
        /// </summary>
        /// <param name="shardingDbContext"></param>
        /// <returns></returns>
        public static bool ReadWriteSeparationWriteOnly(this IShardingDbContext shardingDbContext)
        {
            if (shardingDbContext is ISupportShardingReadWrite supportShardingReadWrite)
            {
                supportShardingReadWrite.SetReadWriteSeparation(false);
                return true;
            }

            return false;
        }
        /// <summary>
        /// 设置读写分离读读数据库
        /// </summary>
        /// <param name="shardingDbContext"></param>
        /// <returns></returns>
        public static bool ReadWriteSeparationReadOnly(this IShardingDbContext shardingDbContext)
        {
            if (shardingDbContext is ISupportShardingReadWrite supportShardingReadWrite)
            {
                supportShardingReadWrite.SetReadWriteSeparation(true);
                return true;
            }

            return false;
        }
        /// <summary>
        /// 设置读写分离
        /// </summary>
        /// <param name="supportShardingReadWrite"></param>
        /// <param name="readOnly">是否是读数据源</param>
        private static void SetReadWriteSeparation(this ISupportShardingReadWrite supportShardingReadWrite, bool readOnly)
        {

            var shardingReadWriteManager = ShardingContainer.GetService<IShardingReadWriteManager>();
            var shardingReadWriteContext = shardingReadWriteManager.GetCurrent(supportShardingReadWrite.GetType());
            if (shardingReadWriteContext != null)
            {
                if (shardingReadWriteContext.DefaultPriority > supportShardingReadWrite.ReadWriteSeparationPriority)
                {
                    supportShardingReadWrite.ReadWriteSeparationPriority = shardingReadWriteContext.DefaultPriority + 1;
                }
            }
            if (supportShardingReadWrite.ReadWriteSeparation!= readOnly)
            {
                supportShardingReadWrite.ReadWriteSeparation = readOnly;
            }
        }
    }
}
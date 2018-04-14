using System;
using System.Data;
using System.Collections.Generic;
using Hidistro.Core;

namespace Chenduo.BLL
{
    /// <summary>
    /// ���ظ���
    /// </summary>
    public partial class article_attach
    {
        private readonly DAL.article_attach dal;
        public article_attach()
        {
            dal = new DAL.article_attach("dt_");
        }
        #region  Method
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// ��ȡ���ش���
        /// </summary>
        public int GetDownNum(int id)
        {
            return dal.GetDownNum(id);
        }

        /// <summary>
        /// �޸�һ������
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            dal.UpdateField(id, strValue);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.article_attach GetModel(int id)
        {
            return dal.GetModel(id);
        }

        //ɾ�����µľ��ļ�
        public void DeleteFile(int id, string filePath)
        {
            Model.article_attach model = GetModel(id);
            if (model != null && model.file_path != filePath)
            {
                SFUtils.DeleteFile(model.file_path);
            }
        }

        #endregion  Method

    }
}
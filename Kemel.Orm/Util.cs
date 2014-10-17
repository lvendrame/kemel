using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Entity;
using Kemel.Orm.Constants;
using System.Data;
using System.Reflection;
using Kemel.Orm.Schema;

namespace Kemel.Orm
{
    public static class Util
    {
        #region Properties Attributes Util

        //public static EntityPK GetParent<EntityPK, EntityFK>(ITier pdtt_Tier, EntityFK ett_Entity)
        //    where EntityFK : EntityBase, new()
        //    where EntityPK : EntityBase, new()
        //{
        //    PropertyInfo propFK = Util.GetForeignKey<EntityFK, EntityPK>();
        //    if (propFK != null)
        //    {
        //        EntityPK ettPK = new EntityPK();
        //        PropertyInfo propPK = GetPrimaryKey<EntityPK>();
        //        propPK.SetValue(ettPK, propFK.GetValue(ett_Entity, null), null);
        //        EntityList<EntityPK> lstEttPK = pdtt_Tier.GetData<EntityPK>(ettPK);
        //        if (lstEttPK.Count != 0)
        //            return lstEttPK[0];
        //        else
        //            throw new HiPersistException(
        //                string.Format("Não foi encontrado nenhum registro de \"{0}\" que coincida com a chave existênte para a entidade \"{1}\"",
        //                typeof(EntityPK).Name, typeof(EntityFK).Name));
        //    }
        //    else
        //    {
        //        throw new HiPersistException(
        //            string.Format("A entidade \"{0}\" não possui nenhum relacionamento com a entidade \"{1}\". Crie um relacionamento ou selecione as entidades corretas.",
        //            typeof(EntityFK).Name, typeof(EntityPK).Name));
        //    }
        //}

        //public static EntityList<EntityFK> GetChildren<EntityPK, EntityFK>(ITier pdtt_Tier, EntityPK ett_Entity)
        //    where EntityFK : EntityBase, new()
        //    where EntityPK : EntityBase, new()
        //{
        //    PropertyInfo propFK = Util.GetForeignKey<EntityFK, EntityPK>();
        //    if (propFK != null)
        //    {
        //        EntityFK ettFK = new EntityFK();
        //        PropertyInfo propPK = GetPrimaryKey<EntityPK>();
        //        propFK.SetValue(ettFK, propPK.GetValue(ett_Entity, null), null);
        //        return pdtt_Tier.GetData<EntityFK>(ettFK);
        //    }
        //    else
        //    {
        //        throw new HiPersistException(
        //            string.Format("A entidade \"{0}\" não possui nenhum relacionamento com a entidade \"{1}\". Crie um relacionamento ou selecione as entidades corretas.",
        //            typeof(EntityFK).Name, typeof(EntityPK).Name));
        //    }
        //}

        #endregion

        #region Populates

        #region FillDataTable
        public static int FromDataReader(this DataTable self, IDataReader dr)
        {
            List<ItemDataReaderToDataTableFill> lstItems = new List<ItemDataReaderToDataTableFill>();
            ItemDataReaderToDataTableFill item = null;
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string name = dr.GetName(i);
                if (!self.Columns.Contains(name))
                {
                    item = new ItemDataReaderToDataTableFill();
                    item.Column = self.Columns.Add(name, dr.GetFieldType(i));
                    item.IndexDR = i;
                    lstItems.Add(item);
                }
            }

            while (dr.Read())
            {
                DataRow row = self.NewRow();

                foreach (ItemDataReaderToDataTableFill idrtdtf in lstItems)
                {
                    row[idrtdtf.Column] = dr.GetValue(idrtdtf.IndexDR);
                }

                self.Rows.Add(row);
            }

            return dr.RecordsAffected;
        }
        #endregion

        #endregion

        internal class ItemDataReaderToDataTableFill
        {
            private int intIndexDR;
            public int IndexDR
            {
                get { return intIndexDR; }
                set { intIndexDR = value; }
            }

            private DataColumn dcColumn;
            public DataColumn Column
            {
                get { return dcColumn; }
                set { dcColumn = value; }
            }
        }


    }
}

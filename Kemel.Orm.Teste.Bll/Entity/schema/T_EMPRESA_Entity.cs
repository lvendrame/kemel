//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:2.0.50727.4952
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kemel.Orm.Teste.Bll.Entity {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Kemel.Orm;
    using Kemel.Orm.Base;
    using Kemel.Orm.Schema;
    using Kemel.Orm.Entity.Attributes;


    public partial class T_EMPRESA_Entity {

        public class Schema {

            private static Kemel.Orm.Schema.TableSchema f_Table = null;

            private static Kemel.Orm.Schema.ColumnSchema f_CODIGO = null;

            private static Kemel.Orm.Schema.ColumnSchema f_NOME = null;

            private static Kemel.Orm.Schema.ColumnSchema f_DESCRICAO = null;

            private static Kemel.Orm.Schema.ColumnSchema f_TIPO_EMPRESA = null;

            private static Kemel.Orm.Schema.ColumnSchema f_INATIVO = null;

            public static Kemel.Orm.Schema.TableSchema Table {
                get {
                    if ((f_Table == null)) {
                        f_Table = SchemaContainer.GetSchema<T_EMPRESA_Entity>();
                    }
                    return Schema.f_Table;
                }
            }

            public static Kemel.Orm.Schema.ColumnSchema CODIGO {
                get {
                    if ((f_CODIGO == null)) {
                        f_CODIGO = Schema.Table.Columns["CODIGO"];
                    }
                    return Schema.f_CODIGO;
                }
            }

            public static Kemel.Orm.Schema.ColumnSchema NOME {
                get {
                    if ((f_NOME == null)) {
                        f_NOME = Schema.Table.Columns["NOME"];
                    }
                    return Schema.f_NOME;
                }
            }

            public static Kemel.Orm.Schema.ColumnSchema DESCRICAO {
                get {
                    if ((f_DESCRICAO == null)) {
                        f_DESCRICAO = Schema.Table.Columns["DESCRICAO"];
                    }
                    return Schema.f_DESCRICAO;
                }
            }

            public static Kemel.Orm.Schema.ColumnSchema TIPO_EMPRESA {
                get {
                    if ((f_TIPO_EMPRESA == null)) {
                        f_TIPO_EMPRESA = Schema.Table.Columns["TIPO_EMPRESA"];
                    }
                    return Schema.f_TIPO_EMPRESA;
                }
            }

            public static Kemel.Orm.Schema.ColumnSchema INATIVO {
                get {
                    if ((f_INATIVO == null)) {
                        f_INATIVO = Schema.Table.Columns["INATIVO"];
                    }
                    return Schema.f_INATIVO;
                }
            }
        }
    }
}

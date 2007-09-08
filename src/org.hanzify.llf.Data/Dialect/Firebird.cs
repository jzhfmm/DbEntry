
using System;
using System.Collections.Generic;
using System.Text;
using Lephone.Data.SqlEntry;

namespace Lephone.Data.Dialect
{
    public class Firebird : SequencedDialect
    {
        public Firebird()
        {
            TypeNames[DataType.Boolean] = "smallint";
            TypeNames[DataType.Date] = "timestamp";
            TypeNames[DataType.String] = "BLOB SUB_TYPE 1";
            TypeNames[DataType.Binary] = "BLOB SUB_TYPE 0";
        }

        protected override string GetSelectSequenceSql(string TableName)
        {
            return string.Format("select gen_id(GEN_{0}_ID, 1) from RDB$DATABASE", TableName.ToUpper());
        }

        public override bool NeedCommitCreateFirst
        {
            get { return true; }
        }

        public override bool SupportDirctionOfEachColumnInIndex
        {
            get { return false; }
        }

        public override string UnicodeTypePrefix
        {
            get { return ""; }
        }

        public override string IdentityColumnString
        {
            get { return "NOT NULL"; }
        }

        public override string PrimaryKeyString
        {
            get { return "PRIMARY KEY"; }
        }

        public override string GetCreateSequenceString(string TableName)
        {
            return string.Format("CREATE GENERATOR GEN_{0}_ID;\n", TableName.ToUpper());
        }

        protected override string QuoteSingle(string name)
        {
            return base.QuoteSingle(name.ToUpper());
        }

        public override string NullString
        {
            get { return ""; }
        }

        public override bool ExecuteEachLine
        {
            get { return true; }
        }

        public override void ExecuteDropSequence(DataProvider dp, string TableName)
        {
            string sql = string.Format("DROP GENERATOR GEN_{0}_ID;\n", TableName.ToUpper());
            dp.ExecuteNonQuery(sql);
        }

        protected override SqlStatement GetPagedSelectSqlStatement(Lephone.Data.Builder.SelectStatementBuilder ssb)
        {
            SqlStatement Sql = base.GetNormalSelectSqlStatement(ssb);
            Sql.SqlCommandText = string.Format("{0} Rows {1} to {2}",
                Sql.SqlCommandText, ssb.Range.StartIndex, ssb.Range.EndIndex);
            return Sql;
        }

        public override bool SupportsRangeStartIndex
        {
            get { return true; }
        }
    }
}
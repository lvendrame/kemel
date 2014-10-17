using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kemel.Tools.Orm
{
    public class DirectoryOrmStructure
    {
        public DirectoryOrmStructure(string originalPath, string entityNameSpace, string dalNameSpace, string businessNameSpace)
        {
            this.OriginalPath = originalPath;
            this.EntityNameSpace = entityNameSpace;
            this.DalNameSpace = dalNameSpace;
            this.BusinessNameSpace = businessNameSpace;
        }

        private DirectoryOrmStructure(string originalPath, string entityNameSpace, string dalNameSpace, string businessNameSpace, bool isSub)
            :this(originalPath, entityNameSpace, dalNameSpace, businessNameSpace)
        {
            this.isSub = isSub;
        }

        public string EntityNameSpace { get; set; }
        public string DalNameSpace { get; set; }
        public string BusinessNameSpace { get; set; }

        public string OriginalPath { get; private set; }

        private bool isSub = false;

        private DirectoryInfo driRoot = null;
        public DirectoryInfo Root {
            get
            {
                if(driRoot == null)
                    driRoot = new DirectoryInfo(this.OriginalPath);
                return driRoot;
            }
        }

        private DirectoryInfo driEntity = null;
        public DirectoryInfo Entity
        {
            get
            {
                if (driEntity == null)
                    driEntity = Root.CreateSubdirectory(this.EntityNameSpace);
                return driEntity;
            }
        }

        private DirectoryInfo driSchema = null;
        public DirectoryInfo Schema
        {
            get
            {
                if (driSchema == null)
                    driSchema = Entity.CreateSubdirectory("Schema");
                return driSchema;
            }
        }

        private DirectoryInfo driDefinition = null;
        public DirectoryInfo Definition
        {
            get
            {
                if (driDefinition == null)
                    driDefinition = Entity.CreateSubdirectory("Definition");
                return driDefinition;
            }
        }

        private DirectoryInfo driExtension = null;
        public DirectoryInfo Extension
        {
            get
            {
                if (driExtension == null)
                    driExtension = Entity.CreateSubdirectory("Extension");
                return driExtension;
            }
        }

        private DirectoryInfo driDal = null;
        public DirectoryInfo Dal
        {
            get
            {
                if (driDal == null)
                    driDal = Root.CreateSubdirectory(this.DalNameSpace);
                return driDal;
            }
        }

        private DirectoryInfo driBusiness = null;
        public DirectoryInfo Business
        {
            get
            {
                if (driBusiness == null)
                    driBusiness = Root.CreateSubdirectory(this.BusinessNameSpace);
                return driBusiness;
            }
        }

        private DirectoryOrmStructure dosView = null;
        public DirectoryOrmStructure View
        {
            get
            {
                if (dosView == null && !isSub)
                {
                    DirectoryInfo rootView = this.Root.CreateSubdirectory("View");
                    dosView = new DirectoryOrmStructure(rootView.FullName, this.EntityNameSpace, this.DalNameSpace, this.BusinessNameSpace, true);
                }
                return dosView;
            }
        }

        private DirectoryOrmStructure dosProcedure = null;
        public DirectoryOrmStructure Procedure
        {
            get
            {
                if (dosProcedure == null && !isSub)
                {
                    DirectoryInfo rootProc = this.Root.CreateSubdirectory("Procedure");
                    dosProcedure = new DirectoryOrmStructure(rootProc.FullName, this.EntityNameSpace, this.DalNameSpace, this.BusinessNameSpace, true);
                }
                return dosProcedure;
            }
        }

        private DirectoryOrmStructure dosFunction = null;
        public DirectoryOrmStructure Function
        {
            get
            {
                if (dosFunction == null && !isSub)
                {
                    DirectoryInfo rootFunction = this.Root.CreateSubdirectory("Function");
                    dosFunction = new DirectoryOrmStructure(rootFunction.FullName, this.EntityNameSpace, this.DalNameSpace, this.BusinessNameSpace, true);
                }
                return dosFunction;
            }
        }

        public void CleanDirectories()
        {
            this.CleanDirectory(this.Schema);
            this.CleanDirectory(this.Definition);
            this.CleanDirectory(this.Extension);
            this.CleanDirectory(this.Entity);
            this.CleanDirectory(this.Dal);
            this.CleanDirectory(this.Business);

            if (!isSub)
            {
                this.View.CleanDirectories();
                this.Procedure.CleanDirectories();
            }
        }

        private void CleanDirectory(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }
    }
}

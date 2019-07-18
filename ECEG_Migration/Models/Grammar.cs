using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Grammar
    {
        private int grammarId;
        private Author grammarAuthor;
        private Imprint grammarImprint;
        private Reference[] grammarReferences;
        private Library[] grammarHoldingLibraries;
        private TypeOfWork grammarTypeOfWork;
        private GrammaticalCategory grammarGrammaticalCategory;
        private SubsidiaryContent[] grammarSubsidiaryContents;
        private TargetAudience grammarTargetAge;
        private TargetAudience grammarTargetGender;
        private TargetAudience grammarTargetInstruction;
        private TargetAudience grammarTargetSP;
        private string grammarCommments;

        public int GrammarId { get => grammarId; set => grammarId = value; }
        public Author GrammarAuthor { get => grammarAuthor; set => grammarAuthor = value; }
        public Imprint GrammarImprint { get => grammarImprint; set => grammarImprint = value; }
        public Reference[] GrammarReferences { get => grammarReferences; set => grammarReferences = value; }
        public Library[] GrammarHoldingLibraries { get => grammarHoldingLibraries; set => grammarHoldingLibraries = value; }
        public TypeOfWork GrammarTypeOfWork { get => grammarTypeOfWork; set => grammarTypeOfWork = value; }
        public GrammaticalCategory GrammarGrammaticalCategory { get => grammarGrammaticalCategory; set => grammarGrammaticalCategory = value; }
        public SubsidiaryContent[] GrammarSubsidiaryContents { get => grammarSubsidiaryContents; set => grammarSubsidiaryContents = value; }
        public TargetAudience GrammarTargetAge { get => grammarTargetAge; set => grammarTargetAge = value; }
        public TargetAudience GrammarTargetGender { get => grammarTargetGender; set => grammarTargetGender = value; }
        public TargetAudience GrammarTargetInstruction { get => grammarTargetInstruction; set => grammarTargetInstruction = value; }
        public TargetAudience GrammarTargetSP { get => grammarTargetSP; set => grammarTargetSP = value; }
        public string GrammarCommments { get => grammarCommments; set => grammarCommments = value; }

        public Grammar(string grammarId)
        {
            this.grammarId = Convert.ToInt32(grammarId);
            this.grammarAuthor = DbManager.GetAuthorDataFromGrammar(grammarId);
            this.grammarImprint = DbManager.GetImprintDataFromGrammar(grammarId);
            this.grammarReferences = DbManager.GetReferenceDataFromGrammar(grammarId);
            this.grammarHoldingLibraries = DbManager.GetHoldingLibrariesFromGrammar(grammarId);
            this.grammarTypeOfWork = DbManager.GetTypeOfWorkFromGrammar(grammarId);
            this.grammarGrammaticalCategory = DbManager.GetGrammaticalCategoryFromGrammar(grammarId);
            this.grammarSubsidiaryContents = DbManager.GetSubsidiaryContentsFromGrammar(grammarId);
            (this.grammarTargetAge, this.grammarTargetGender, this.grammarTargetInstruction, this.grammarTargetSP) = DbManager.GetAudienceCriteriasFromGrammar(grammarId);
            this.grammarCommments = DbManager.getCommentsFromGrammar(grammarId);
        }
    }
}
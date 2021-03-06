﻿using System;
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
        private GrammarDivision grammarDivision;
        private SubsidiaryContent[] grammarSubsidiaryContents;
        private TargetAudience grammarTargetAge;
        private TargetAudience grammarTargetGender;
        private TargetAudience grammarTargetInstruction;
        private TargetAudience grammarTargetSP;
        private string grammarCommments;
        private string grammarPublicationYear;
        private string grammarTitle;
        private Edition[] grammarEditions;
        private int grammarFirstEdition;
        public int GrammarId { get => grammarId; set => grammarId = value; }
        public Author GrammarAuthor { get => grammarAuthor; set => grammarAuthor = value; }
        public Imprint GrammarImprint { get => grammarImprint; set => grammarImprint = value; }
        public Reference[] GrammarReferences { get => grammarReferences; set => grammarReferences = value; }
        public Library[] GrammarHoldingLibraries { get => grammarHoldingLibraries; set => grammarHoldingLibraries = value; }
        public TypeOfWork GrammarTypeOfWork { get => grammarTypeOfWork; set => grammarTypeOfWork = value; }
        public GrammarDivision GrammarDivision { get => grammarDivision; set => grammarDivision = value; }
        public SubsidiaryContent[] GrammarSubsidiaryContents { get => grammarSubsidiaryContents; set => grammarSubsidiaryContents = value; }
        public TargetAudience GrammarTargetAge { get => grammarTargetAge; set => grammarTargetAge = value; }
        public TargetAudience GrammarTargetGender { get => grammarTargetGender; set => grammarTargetGender = value; }
        public TargetAudience GrammarTargetInstruction { get => grammarTargetInstruction; set => grammarTargetInstruction = value; }
        public TargetAudience GrammarTargetSP { get => grammarTargetSP; set => grammarTargetSP = value; }
        public string GrammarCommments { get => grammarCommments; set => grammarCommments = value; }
        public string GrammarPublicationYear { get => grammarPublicationYear; set => grammarPublicationYear = value; }
        public string GrammarTitle { get => grammarTitle; set => grammarTitle = value; }
        public Edition[] GrammarEditions { get => grammarEditions; set => grammarEditions = value; }
        public int GrammarFirstEdition { get => grammarFirstEdition; set => grammarFirstEdition = value; }

        public Grammar(string grammarId)
        {
            this.grammarId = Convert.ToInt32(grammarId);
            this.grammarAuthor = DbManager.GetAuthorDataFromGrammar(grammarId);
            this.grammarImprint = DbManager.GetImprintDataFromGrammar(grammarId);
            this.grammarReferences = DbManager.GetReferenceDataFromGrammar(grammarId);
            this.grammarHoldingLibraries = DbManager.GetHoldingLibrariesFromGrammar(grammarId);
            this.grammarTypeOfWork = DbManager.GetTypeOfWorkFromGrammar(grammarId);
            this.grammarDivision = DbManager.GetGrammaticalCategoryFromGrammar(grammarId);
            this.grammarSubsidiaryContents = DbManager.GetSubsidiaryContentsFromGrammar(grammarId);
            //(this.grammarTargetAge, this.grammarTargetGender, this.grammarTargetInstruction, this.grammarTargetSP) = DbManager.GetAudienceCriteriasFromGrammar(grammarId);
            (this.grammarCommments, this.GrammarPublicationYear, this.GrammarTitle) = DbManager.getBasicInfoFromGrammar(grammarId);
        }

        public Grammar()
        {
        }
        public Grammar CustomClone()
        {
            Grammar g = new Grammar
            {
                grammarId = this.grammarId,
                grammarAuthor = this.grammarAuthor,
                grammarImprint = this.grammarImprint,
                grammarReferences = this.grammarReferences,
                grammarHoldingLibraries = this.grammarHoldingLibraries,
                grammarTypeOfWork = this.grammarTypeOfWork,
                grammarDivision = this.grammarDivision,
                grammarSubsidiaryContents = this.grammarSubsidiaryContents,
                grammarTargetAge = this.grammarTargetAge,
                grammarTargetGender = this.grammarTargetGender,
                grammarTargetInstruction = this.grammarTargetInstruction,
                grammarTargetSP = this.grammarTargetSP,
                grammarCommments = this.grammarCommments,
                grammarPublicationYear = this.grammarPublicationYear,
                grammarTitle = this.grammarTitle,
                grammarEditions = this.grammarEditions,
                grammarFirstEdition = this.grammarFirstEdition
            };
            return g;
        }
    }

}
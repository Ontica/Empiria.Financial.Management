/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : FinancialConcept                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial concept.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Financial.Concepts {

  /// <summary>Represents a financial concept.</summary>
  public class FinancialConcept : BaseObject, INamedEntity {

    #region Constructors and parsers

    private FinancialConcept() {
      // Require by Empiria FrameWork
    }

    static internal FinancialConcept Parse(int id) => ParseId<FinancialConcept>(id);

    static internal FinancialConcept Parse(string uid) => ParseKey<FinancialConcept>(uid);

    static public FinancialConcept Empty => ParseEmpty<FinancialConcept>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("CPT_GROUP_ID")]
    public FinancialConceptGroup Group {
      get; private set;
    }


    [DataField("CPT_NO")]
    public string ConceptNo {
      get; private set;
    }


    [DataField("CPT_NAME")]
    public string Name {
      get; private set;
    }


    [DataField("CPT_DESCRIPTION")]
    public string Description {
      get; private set;
    }

    public string FullName {
      get {
        if (this.IsEmptyInstance) {
          return string.Empty;
        }
        if (Parent.FullName.Length == 0) {
          return Description;
        }
        return $"{Description} - {Parent.FullName}";
      }
    }


    [DataField("CPT_ROLE", Default = AccountRoleType.Undefined)]
    public AccountRoleType RoleType {
      get; private set;
    }


    [DataField("CPT_VARIABLE_NAME")]
    public string VariableName {
      get; private set;
    }


    [DataField("CPT_SCRIPT")]
    public string Script {
      get; private set;
    }


    [DataField("CPT_RULE_DATA")]
    public string RuleData {
      get; private set;
    }


    [DataField("CPT_EXT_DATA")]
    public JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return $"{ConceptNo} " + EmpiriaString.BuildKeywords(Name, Description, Group.Name);
      }
    }


    [DataField("CPT_POSITION")]
    public int Position {
      get; private set;
    }


    [DataField("CPT_PARENT_ID")]
    private int _parentId = -1;

    public FinancialConcept Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
      private set {
        if (this.IsEmptyInstance) {
          return;
        }
        _parentId = value.Id;
      }
    }


    [DataField("CPT_START_DATE", Default = "ExecutionServer.DateMinValue")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("CPT_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("CPT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CPT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CPT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }


    public int Level {
      get {
        return EmpiriaString.CountOccurences(ConceptNo, '.') + 1;
      }
    }


    public bool IsLastLevel {
      get {
        return RoleType != AccountRoleType.Sumaria;
      }
    }

    #endregion Properties

    #region Methods

    private FixedList<FinancialConcept> _allChildren = null;
    internal FixedList<FinancialConcept> GetAllChildren() {
      if (_allChildren == null) {
        _allChildren = GetFullList<FinancialConcept>()
                      .ToFixedList()
                      .FindAll(x => x.ConceptNo.StartsWith($"{this.ConceptNo}.") &&
                                   !x.IsEmptyInstance)
                                   .Sort((x, y) => x.ConceptNo.CompareTo(y.ConceptNo)
                      );
      }
      return _allChildren;
    }


    internal FixedList<FinancialConcept> GetChildren() {
      return GetAllChildren()
            .FindAll(x => x.Parent.Equals(this));
    }

    #endregion Methods

  } // class FinancialConcept

} // namespace Empiria.Financial.Concepts

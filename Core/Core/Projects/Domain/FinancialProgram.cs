/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned type                        *
*  Type     : FinancialProgram                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial program.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Projects {

  /// <summary>Represents a financial program.</summary>
  public class FinancialProgram : BaseObject, INamedEntity {

    // static private readonly FixedList<StandardAccount> _allSubprograms;

    #region Constructors and parsers

    static public FinancialProgram Parse(int id) => ParseId<FinancialProgram>(id);

    static public FinancialProgram Parse(string uid) => ParseKey<FinancialProgram>(uid);

    static public FinancialProgram Empty => ParseEmpty<FinancialProgram>();


    static public FixedList<FinancialProgram> GetList() {
      return GetFullList<FinancialProgram>()
             .FindAll(x => !x.IsEmptyInstance);

    }


    static public FixedList<FinancialProgram> GetList(FinancialProgramType type) {
      return GetFullList<FinancialProgram>()
            .FindAll(x => x.FinancialProgramType == type);
    }


    static public FixedList<FinancialProgram> GetList(FinancialProjectCategory category) {
      return GetList(FinancialProgramType.Subprograma)
             .FindAll(x => x.ProgramNo.Substring(3) == category.StandardAccountCode)
             .ToFixedList()
             .SelectDistinct(x => x.Parent);
    }

    #endregion Constructors and parsers


    public FinancialProgramType FinancialProgramType {
      get {
        return (FinancialProgramType) Enum.Parse(typeof(FinancialProgramType), Level.ToString());
      }
    }

    [DataField("PROGRAM_NO")]
    public string ProgramNo {
      get; private set;
    }


    [DataField("PROGRAM_NAME")]
    public string Name {
      get; private set;
    }

    [DataField("PROGRAM_PARENT_ID")]
    private int _parentId = -1;

    public FinancialProgram Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
    }


    private FixedList<FinancialProgram> _children = null;
    public FixedList<FinancialProgram> Children {
      get {
        if (this.IsEmptyInstance) {
          return new FixedList<FinancialProgram>();
        }
        if (_children == null) {
          _children = GetFullList<FinancialProgram>()
                     .FindAll(x => x._parentId == this.Id);
        }
        return _children;
      }
    }


    public string Keywords {
      get {
        if (this.IsEmptyInstance) {
          return string.Empty;
        }
        return EmpiriaString.BuildKeywords(ProgramNo, Name, Parent.Keywords);
      }
    }


    public int Level {
      get {
        if (this.IsEmptyInstance) {
          return 0;
        }
        return EmpiriaString.CountOccurences(ProgramNo, '.') + 1;
      }
    }

  } // class FinancialProgram

} // namespace Empiria.Financial.Projects

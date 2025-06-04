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

    #region Constructors and parsers

    static internal FinancialProgram Parse(int id) => ParseId<FinancialProgram>(id);

    static internal FinancialProgram Parse(string uid) => ParseKey<FinancialProgram>(uid);

    static internal FinancialProgram Empty => ParseEmpty<FinancialProgram>();


    static internal FixedList<FinancialProgram> GetList() {
      return GetFullList<FinancialProgram>()
             .FindAll(x => !x.IsEmptyInstance);
    }


    static internal FixedList<FinancialProgram> GetList(FinancialProgramType type) {
      return GetList()
            .FindAll(x => x.FinancialProgramType == type);
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
    internal int _parentId = -1;

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
          _children = GetList()
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


    public StandardAccount StandardAccount {
      get {
        return StandardAccount.Parse(Id);
      }
    }

  } // class FinancialProgram

} // namespace Empiria.Financial.Projects

/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountSegment                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an standard account segment.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Represents an standard account segment.</summary>
  public class StandardAccountSegment : CommonStorage, INamedEntity {

    #region Constructors and parsers

    protected StandardAccountSegment() {
      // Required by Empiria Framework
    }

    static public StandardAccountSegment Parse(int id) => ParseId<StandardAccountSegment>(id);

    static public StandardAccountSegment Parse(string uid) => ParseKey<StandardAccountSegment>(uid);

    static public StandardAccountSegment Parse(StandardAccountCategory category, string segmentCode) {
      var segment = GetList().Find(x => x.Category.Equals(category) && x.Code == segmentCode);

      Assertion.Require(segment, $"Standard account segment code '{segmentCode}' is not defined " +
                                 $"for category '{category.Name}'.");


      return segment;
    }

    static public StandardAccountSegment Empty => ParseEmpty<StandardAccountSegment>();

    static public FixedList<StandardAccountSegment> GetList() {
      return GetStorageObjects<StandardAccountSegment>();
    }

    static public FixedList<StandardAccountSegment> GetList(StandardAccountCategory category) {
      return GetList().FindAll(x => x.Category.Equals(category))
                      .Sort((x, y) => x.Code.CompareTo(y.Code));
    }

    #endregion Constructors and parsers

    #region Properties

    public StandardAccountCategory Category {
      get {
        return base.GetCategory<StandardAccountCategory>();
      }
      private set {
        base.SetCategory(value);
      }
    }


    public new string Code {
      get {
        return base.Code;
      }
      private set {
        base.Code = value;
      }
    }

    string INamedEntity.Name {
      get {
        return $"({Code}) {base.Name}";
      }
    }

    #endregion Properties

  } // class StandardAccountSegment

} // namespace Empiria.Financial

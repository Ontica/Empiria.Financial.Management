/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashflowProjectionCategory                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cashflow projection category that serves to classify CashflowProjectionTypes.     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Cashflow.Projections {

  /// <summary>Represents a cashflow projection category that serves to
  /// classify CashflowProjectionTypes.</summary>
  public class CashflowProjectionCategory : CommonStorage {

    #region Constructors and parsers

    private CashflowProjectionCategory() {
      // Required by Empiria Framework.
    }


    static public CashflowProjectionCategory Parse(int id) => ParseId<CashflowProjectionCategory>(id);

    static public CashflowProjectionCategory Parse(string uid) => ParseKey<CashflowProjectionCategory>(uid);

    static public FixedList<CashflowProjectionCategory> GetList() {
      return BaseObject.GetList<CashflowProjectionCategory>(string.Empty, "Object_Name")
                       .FindAll(x => x.Status != EntityStatus.Deleted)
                       .ToFixedList();
    }


    static public CashflowProjectionCategory Empty => ParseEmpty<CashflowProjectionCategory>();

    #endregion Constructors and parsers

    #region Properties

    public CashflowProjectionType ProjectionType {
      get {
        if (IsEmptyInstance) {
          return CashflowProjectionType.Empty;
        }

        int id = ExtData.Get<int>("projectionTypeId");

        return ObjectTypeInfo.Parse<CashflowProjectionType>(id);
      }
    }


    public EntityStatus Status {
      get {
        return base.GetStatus<EntityStatus>();
      }
    }

    #endregion Properties

  }  // class CashflowProjectionCategory

}  // namespace Empiria.Cashflow.Projections

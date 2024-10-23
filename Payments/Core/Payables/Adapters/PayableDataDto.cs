/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PayableDataDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payable data structure.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Summary  : Data transfer objects used to return payable data structure.</summary>
  public class PayableDataDto {

    public PayableDto Payable {
      get; internal set;
    }
    
    public PayableEntityDto PayableEntity {
      get; internal set;
    }


    public FixedList<PayableDataItemDto> Items {
      get; internal set;
    }


    public FixedList<DocumentDto> Documents {
      get; internal set;
    }


    public FixedList<HistoryDto> History {
      get; internal set;
    }
    

    public ActionsDto Actions {
      get; internal set;
    } 

  } // class PayableDataDto


  // <summary>Output DTO used to return payable data item minimal information.</summary>
  public class PayableDataItemDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string BudgetAccount {
      get; internal set;
    }


    public string BillConcept {
      get; internal set;
    }


    public string PayableEntityItemUID {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }


    public string Unit {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


  } // class PayableEntityItemDto


  /// <summary>Output DTO used to return payable type general information.</summary>
  public class PayableEntityDto {

    public string UID {
      get; internal set;
    }


    public string Type {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }

    //TODO 
    //public AttributesDto Attributes {
    //  get; internal set;
    //}
       

    public FixedList<PayableEntityItemDto> Items {
      get; internal set;
    }


  } // class PayableEntityDto


  // <summary>Output DTO used to return payable entity item minimal information.</summary>
  public class PayableEntityItemDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }


    public string Unit {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }

  } // class PayableEntityItemDto


  // <summary>Output DTO used to return document information about payables.</summary>
  public class DocumentDto {

    public string UID {
      get; internal set;
    }


    public string Type {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public DateTime UploadedDate {
      get; internal set;
    }


    //? if is all party or EntityName or string
    public string UploadedBy {
      get; internal set;
    }


    //TODO
    //public DocumentAttributesDto Attributes {
    //  get; internal set;
    //}

  } // class DocumentDto


  // <summary>Output DTO used to return minimal history information about payables.</summary>
  public class HistoryDto {

    public string UID {
      get; internal set;
    }


    public string Type {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public DateTime Time {
      get; internal set;
    }


    public NamedEntityDto Party {
      get; internal set;
    }


  } // HistoryDto


  // <summary>Output DTO used to return ActionsDto. </summary>
  public class ActionsDto {

    public CanDto CanObject {
      get; internal set;
    }

    public ShowDto ShowObject {
      get; internal set;
    }

  } // class TransactionActionDto

  //Class to set order permissions
  public class CanDto {
    
    
    public Boolean Cancel {
      get; internal set;
    }


    public Boolean Update {
      get; internal set;
    }

  } // class CanDto

  // class to show or hide interface items
  public class ShowDto {

    public Boolean PayablesData {
      get; internal set;
    }


    public Boolean PaymentsData {
      get; internal set;
    }


  } //class ShowDto


}  // namespace Empiria.Payments.Payables.Adapters

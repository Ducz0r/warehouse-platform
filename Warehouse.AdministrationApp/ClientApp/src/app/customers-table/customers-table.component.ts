import { Component, OnInit } from '@angular/core';
import { CustomersService } from '../data/customers-service';
import { CustomerModel } from '../data/models/customer.model';

@Component({
  selector: 'customers-table',
  templateUrl: './customers-table.component.html'
})
export class CustomersTableComponent implements OnInit {
  customers: CustomerModel[];

  constructor(public customersService: CustomersService) { }

  ngOnInit() {
    this.customersService
      .getAllCustomers()
      .subscribe((data: CustomerModel[]) => {
        this.customers = data;
      });

  }
}

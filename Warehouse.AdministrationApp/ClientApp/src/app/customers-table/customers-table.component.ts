import { Component, OnDestroy, OnInit } from '@angular/core';
import { interval, startWith, Subscription, switchMap } from 'rxjs';
import { CustomersService } from '../data/customers-service';
import { CustomerModel } from '../data/models/customer.model';

@Component({
  selector: 'customers-table',
  templateUrl: './customers-table.component.html'
})
export class CustomersTableComponent implements OnInit, OnDestroy {
  timeInterval: Subscription;
  customers: CustomerModel[] = [];

  constructor(public customersService: CustomersService) { }

  ngOnInit() {
    this.timeInterval = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.customersService.getAllCustomers())
    ).subscribe((data: CustomerModel[]) => {
      this.customers.splice(0);
      data.forEach((c) => { this.customers.push(c); });
    }, err => { console.log('Error with retrieving server\'s customers'); }
    );
  }

  ngOnDestroy() {
  }
}

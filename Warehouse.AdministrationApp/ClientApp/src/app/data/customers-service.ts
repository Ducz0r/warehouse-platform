import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { CustomerModel } from "./models/customer.model";
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private readonly URL = "https://localhost:5001/customers";
  private readonly BEARER_SECRET = "1f0971861e89567ba2182f11616e97fc5413982ac4074d44aec82d7a37b467f0";
  private headers: HttpHeaders;

  constructor(private httpClient: HttpClient) {
    this.headers = new HttpHeaders();
    this.headers = this.headers.append("Accept", "application/json");
    this.headers = this.headers.append("Authorization", `Bearer ${this.BEARER_SECRET}`);
  }

  getAllCustomers(): Observable<CustomerModel[]> {
    return this.httpClient.get<CustomerModel[]>(
      this.URL,
      { headers: this.headers }
    ).pipe(
      catchError(this.handleError<CustomerModel[]>('customers', []))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.log(`failed: ${error.message}`);
      return of(result as T);
    };
  }
}

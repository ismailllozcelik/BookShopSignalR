import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Book } from '../models/book.model';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class BookService {
    bookUrl = "https://localhost:44343/api/book/";
   
    constructor(private httpClient: HttpClient) { }

    GetBooks(connectionId: string): Observable<any[]> {
        return this.httpClient.get<Book[]>(this.bookUrl+ `${connectionId}`);
    }
}
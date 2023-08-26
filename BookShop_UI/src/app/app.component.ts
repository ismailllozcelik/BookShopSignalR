import { Component, OnInit } from '@angular/core';
import { Book } from './models/book.model';
import { BookService } from './services/book.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Books Shop';
  modelBooks: Book[] = [];

  _hubConnection!: HubConnection;
  _connectionId!: string;
  signalRServiceIp: string = "https://localhost:44343/bookHub";

  public constructor(private service: BookService) {
    this.modelBooks = [];
  }

  public ngOnInit(): void { 
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.signalRServiceIp}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this._hubConnection.start().then(
      () => console.log("Hub Connection Start"))
      .catch(err => console.log(err));

    this._hubConnection.on('GetConnectionId', (connectionId: string) => {
      this._connectionId = connectionId;
      console.log("ConnectionID :" + connectionId);
      this.service.GetBooks(connectionId).subscribe(res => {
        console.log('res', res)
        this.modelBooks = res;
      });
    });

    this._hubConnection.on('ChangeBook', (book: Book) => {
      var item = this.modelBooks.find(rd => rd.name == book.name);
      this.modelBooks = this.modelBooks.filter(gam => gam != item);            
      this.modelBooks.push(book);
    });
  }
}
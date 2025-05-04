# Chat

A real-time chat application built using ASP.NET Core, SignalR, and Angular, designed to facilitate seamless communication between users.

## Table of Contents

- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Usage](#usage)

## Features

- Real-time messaging between users.
- User authentication and authorization.
- Responsive user interface built with Angular.
- Scalable backend using ASP.NET Core and SignalR.

## Stack

- **Backend**: ASP.NET Core
- **Real-time Communication**: SignalR
- **Frontend**: Angular
- **Database**: SQL Server
- **Hosting**: AWS Elastic Beanstalk

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/cli)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup Instructions

1. **Clone the repository**:

   ```bash
   git clone https://github.com/Shchoholiev/chat.git
   cd chat
   ```

2. **Backend Setup**:

   - Navigate to the backend project directory:

     ```bash
     cd Chat
     ```

   - Restore dependencies:

     ```bash
     dotnet restore
     ```

   - Build the project:

     ```bash
     dotnet build
     ```

   - Apply database migrations:

     ```bash
     dotnet ef database update
     ```

   - Run the backend server:

     ```bash
     dotnet run
     ```

3. **Frontend Setup**:

   - Navigate to the frontend project directory:

     ```bash
     cd ClientApp
     ```

   - Install dependencies:

     ```bash
     npm install
     ```

   - Serve the Angular application:

     ```bash
     ng serve
     ```

   The application should now be running at `http://localhost:4200/`.

## Usage

- **User Authentication**: Register and log in to access chat functionalities.
- **Create Chat Rooms**: Users can create new chat rooms for group discussions.
- **Join Existing Chats**: Users can join existing chat rooms to participate in conversations.
- **Real-time Messaging**: Send and receive messages instantly within chat rooms.

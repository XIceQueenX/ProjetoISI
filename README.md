# ISI API – README

## Project Overview

This project was developed as part of the Information Systems Integration (ISI) subject of the Computer Engineering degree. The ISI API is a RESTful API that allows users to manage Books and Movies, perform user authentication, and obtain recommendations between books and movies. The API follows the OpenAPI Specification 3.0 and provides a Swagger UI for testing and documentation.
**Authors:** Gloria Martins, Paula Canuto
**Version:** v1

---

## Technologies & Standards

* RESTful API
* JSON request/response format
* OpenAPI (Swagger)
* Deployed on Azure
* Role-based access control (PUBLIC vs ADMIN)

---

## Base URL

```
https://trabalho-isi-gdf5d0gnh4bwawfw.westeurope-01.azurewebsites.net
```

Swagger documentation is available at:

```
/swagger/v1/swagger.json
```

---

## Authentication

Some endpoints are **PUBLIC**, while others require an **ADMIN** role.

### Register a User

**POST** `/api/auth/register`

Registers a new user.

**Request Body (JSON):**

```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "role": "string"
}
```

**Parameters:**

* `username` – User name
* `email` – User email (used for login)
* `password` – User password
* `role` – User role (e.g., USER or ADMIN)

---

### Login

**POST** `/api/auth/login`

Authenticates a user.

**Request Body (JSON):**

```json
{
  "email": "string",
  "password": "string"
}
```

After login, the user receives authentication data (e.g., token), which must be used to access protected endpoints.

---

## Books Endpoints

### Get All Books (PUBLIC)

**GET** `/api/Books`

**Query Parameters:**

* `page` (integer, default: 1) – Page number
* `pageSize` (integer, default: 10) – Number of items per page
* `search` (string, optional) – Search by title or description

---

### Get Book by ID (PUBLIC)

**GET** `/api/Books/{id}`

**Path Parameter:**

* `id` – Book identifier

---

### Create a Book (ADMIN ONLY)

**POST** `/api/Books`

**Request Body (JSON):**

```json
{
  "title": "string",
  "subtitle": "string",
  "authors": "string",
  "publisher": "string",
  "publishedDate": "string",
  "description": "string"
}
```

---

### Update a Book (ADMIN ONLY)

**PUT** `/api/Books/{id}`

**Path Parameter:**

* `id` – Book identifier

**Request Body:** same as book creation.

---

### Delete a Book (ADMIN ONLY)

**DELETE** `/api/Books/{id}`

---

### Get Books by Author (PUBLIC)

**GET** `/api/Books/by-author/{author}`

**Path Parameter:**

* `author` – Author name

---

### Book Statistics (PUBLIC)

**GET** `/api/Books/stats`

Returns basic statistics about books.

---

## Movies Endpoints

### Get All Movies (PUBLIC)

**GET** `/api/Movies`

**Query Parameters:**

* `page` (integer, default: 1)
* `pageSize` (integer, default: 10)
* `search` (string, optional) – Search by title or original title

---

### Get Movie by ID (PUBLIC)

**GET** `/api/Movies/{id}`

**Path Parameter:**

* `id` – Movie identifier

---

### Create a Movie (ADMIN ONLY)

**POST** `/api/Movies`

**Request Body (JSON):**

```json
{
  "title": "string",
  "originalTitle": "string",
  "overview": "string",
  "releaseDate": "string",
  "posterPath": "string",
  "backdropPath": "string"
}
```

---

### Update a Movie (ADMIN ONLY)

* **PUT** `/api/Movies/{id}` – Full update
* **PATCH** `/api/Movies/{id}` – Partial update

---

### Delete a Movie (ADMIN ONLY)

**DELETE** `/api/Movies/{id}`

---

### Movies by Year (PUBLIC)

**GET** `/api/Movies/by-year/{year}`

**Path Parameter:**

* `year` – Release year

---

### Recent Movies (PUBLIC)

**GET** `/api/Movies/recent`

**Query Parameter:**

* `count` (integer, default: 10) – Number of movies to return

---

## Recommendation Endpoints

### Movies Recommended for a Book

**GET** `/api/Recommendation/movies-for-book/{bookId}`

**Path Parameter:**

* `bookId` – Book identifier

---

### Books Recommended for a Movie

**GET** `/api/Recommendation/books-for-movie/{movieId}`

**Path Parameter:**

* `movieId` – Movie identifier

---

### Personalized Recommendations

**POST** `/api/Recommendation/personalized`

**Request Body (JSON):**

```json
{
  "genre": "string",
  "keywords": ["string"]
}
```

---

## Seed Endpoint

**POST** `/Seed`

Used to populate the database with initial data (for testing or development).

---

## How to Use This API

1. Register a user using `/api/auth/register`
2. Login using `/api/auth/login`
3. Use PUBLIC endpoints to retrieve books and movies
4. Use ADMIN credentials to create, update, or delete data
5. Explore recommendations between books and movies
6. Test all endpoints easily using Swagger UI

---

## Notes

* All request and response bodies use JSON
* ADMIN-only endpoints require proper authorization
* This API was developed for academic purposes 

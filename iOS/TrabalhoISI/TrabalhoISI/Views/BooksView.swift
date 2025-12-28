//
//  BooksView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import SwiftUI

import SwiftUI

struct BooksView: View {
  @StateObject var vm: BooksViewModel = BooksViewModel()
  
  var body: some View {
    NavigationStack {
      List {
        ForEach(vm.books) { book in
          VStack(alignment: .leading, spacing: 4) {
            Text(book.title)
              .font(.headline)
            if let subtitle = book.subtitle {
              Text(subtitle)
                .font(.subheadline)
                .foregroundColor(.gray)
            }
            Text(book.authors)
              .font(.subheadline)
              .foregroundColor(.secondary)
          }
          .padding(.vertical, 4)
          .task {
            await vm.loadMoreIfNeeded(currentBook: book)
          }
        }
        
        if vm.isLoading {
          HStack {
            Spacer()
            ProgressView()
            Spacer()
          }
        }
      }
      .navigationTitle("Books")
      .searchable(text: $vm.searchText, prompt: "Search by author")
      .onChange(of: vm.searchText) { newValue in
        if newValue.isEmpty {
          vm.books = vm.allBooks
        }
      }
      .onSubmit(of: .search) {
        Task { await vm.searchBooksByAuthor() }
      }
      
      .onAppear {
        Task { await vm.loadBooks() }
      }
    }
  }
}


#Preview {
  let vm = BooksViewModel()
  vm.books = [
    Book(
      id: 1,
      title: "1984",
      subtitle: "A Dystopian Novel",
      authors: "George Orwell",
      publisher: "Secker & Warburg",
      publishedDate: "1949",
      description: "A dystopian social science fiction novel..."
    ),
    Book(
      id: 2,
      title: "The Hobbit",
      subtitle: nil,
      authors: "J.R.R. Tolkien",
      publisher: "George Allen & Unwin",
      publishedDate: "1937",
      description: "A fantasy novel and children's book..."
    )
  ]
  return BooksView(vm: vm)
}


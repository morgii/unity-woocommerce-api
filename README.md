# 🛒 Unity + WooCommerce API Integration

A complete integration for connecting **Unity** and **WooCommerce**, allowing Unity applications or games to fetch WooCommerce orders, validate purchases, and communicate with a WordPress store using the **WooCommerce REST API** and **JWT Authentication**.

---

## 📘 Table of Contents
1. [Overview](#-overview)
2. [Features](#-features)
3. [Requirements](#-requirements)
4. [Installation Guide](#-installation-guide)
   - [1. Clone Repository](#1️⃣-clone-repository)
   - [2. WordPress Setup](#2️⃣-wordpress-setup)
   - [3. JWT Authentication Setup](#3️⃣-jwt-authentication-setup)
   - [4. Unity Setup](#4️⃣-unity-setup)
5. [Example Code](#-example-code)
   - [PHP (WordPress Plugin)](#php-wordpress-plugin)
   - [C# (Unity Script)](#c-unity-script)
6. [Common Issues](#-common-issues)
7. [License](#-license)
8. [Credits](#-credits)

---

## 🔍 Overview

This repository bridges **Unity** and **WooCommerce**, allowing developers to:
- Validate in-game purchases against WooCommerce orders.
- Fetch and display order information in Unity.
- Build secure eCommerce-integrated Unity projects.

Communication happens through **WordPress REST API endpoints** secured using **JWT tokens**.

---

## ✨ Features

- ✅ Fetch WooCommerce order data  
- ✅ Validate orders and products  
- ✅ Secure REST API calls using JWT  
- ✅ Works with any Unity version (2020+)  
- ✅ Simple WordPress plugin included  
- ✅ Full C# example provided  

---

## 🧩 Requirements

| Component | Minimum Version | Description |
|------------|-----------------|--------------|
| WordPress  | 6.0+            | Backend store |
| WooCommerce | 8.0+           | E-commerce system |
| PHP        | 7.4+            | Required for plugin |
| Unity      | 2020+           | Client app |
| JWT Plugin | Latest          | For authentication |

---

## ⚙️ Installation Guide

### 1️⃣ Clone Repository

```bash
git clone https://github.com/morgii/unity-woocommerce-api.git

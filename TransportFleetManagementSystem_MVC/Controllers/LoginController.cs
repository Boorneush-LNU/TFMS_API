using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransportFleetManagementSystem.DTOs;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class LoginController : Controller
        {
        private readonly HttpClient _httpClient; 

        public LoginController(IHttpClientFactory httpClientFactory) 
            {
            _httpClient = httpClientFactory.CreateClient(); 
            _httpClient.BaseAddress = new System.Uri("https://localhost:7072/"); 
            }

        public IActionResult Index()
            {
            return View();
            }

        public IActionResult Register()
            {
            return View();
            }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
            {
            try
                {
                var response = await _httpClient.PostAsJsonAsync("api/users/login", loginRequest); // Send login request

                if (response.IsSuccessStatusCode)
                    {
                    // Successful login
                    return RedirectToAction("Index", "Vehicle"); // Redirect to home page or another action
                    }
                else
                    {
                    // Login failed
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = errorMessage; // Display error on the view
                    return View("Index", loginRequest); // Return to login view with error
                    }
                }
            catch (HttpRequestException ex)
                {
                ViewBag.ErrorMessage = "Network error: " + ex.Message;
                return View("Index", loginRequest);
                }
            catch (JsonException ex)
                {
                ViewBag.ErrorMessage = "Json Parsing error: " + ex.Message;
                return View("Index", loginRequest);

                }
            catch (Exception ex)
                {
                ViewBag.ErrorMessage = "An unexpected error occurred: " + ex.Message;
                return View("Index", loginRequest);
                }

            }

        [HttpPost]
        public async Task<IActionResult> Register(User user) // Use User model or a RegisterDto
            {
            try
                {
                var response = await _httpClient.PostAsJsonAsync("api/users/register", user);

                if (response.IsSuccessStatusCode)
                    {
                    return RedirectToAction("Index"); // Redirect to login page or another action
                    }
                else
                    {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = errorMessage;
                    return View("Register", user); // Return to registration view with error
                    }
                }
            catch (HttpRequestException ex)
                {
                ViewBag.ErrorMessage = "Network error: " + ex.Message;
                return View("Register", user);
                }
            catch (JsonException ex)
                {
                ViewBag.ErrorMessage = "Json Parsing error: " + ex.Message;
                return View("Register", user);
                }
            catch (Exception ex)
                {
                ViewBag.ErrorMessage = "An unexpected error occurred: " + ex.Message;
                return View("Register", user);
                }

            }
        }
    }

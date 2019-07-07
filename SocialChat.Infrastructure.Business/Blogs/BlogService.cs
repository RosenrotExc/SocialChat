using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Documents;
using SocialChat.Domain.Core.Infrastructure;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Messages.Blogs;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Services.Interfaces.Blogs;

namespace SocialChat.Infrastructure.Business.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<BaseResponse> CreateBlogAsync(Blog blog, ModelStateDictionary modelState)
        {
            try
            {
                if (blog == null)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest, 
                        Constants.Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errors = modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(err => err.ErrorMessage)
                        .Aggregate((f, s) => $"{f}; {s}");

                    return BaseResponse.Failure(HttpStatusCode.BadRequest, errors);
                }

                if (_blogRepository.CheckForExistedBlog(blog.Title))
                {
                    return BaseResponse.Failure(HttpStatusCode.Conflict,
                        Constants.Validation.Blogs.SameBlogTitleExists());
                }

                blog.CreateDate = DateTime.Now;
                await _blogRepository.CreateBlogAsync(blog);

                return BaseResponse.Success;
            }
            catch (DocumentClientException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.CosmosError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> DeleteBlogAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest,
                        Constants.Validation.Blogs.IncorrectId());
                }

                var deletedCount = await _blogRepository.DeleteBlogAsync(id);

                if (deletedCount > 0)
                {
                    return BaseResponse.Success;
                }

                return BaseResponse.Failure(HttpStatusCode.NotFound,
                    Constants.Validation.Blogs.BlogNotFound(id));
            }
            catch (DocumentClientException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.CosmosError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public BlogResponse GetBlog(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BaseResponse.Failure<BlogResponse>(HttpStatusCode.BadRequest,
                        Constants.Validation.Blogs.IncorrectId());
                }

                var blog = _blogRepository.GetBlog(id);

                if (blog == null)
                {
                    return BaseResponse.Failure<BlogResponse>(HttpStatusCode.NotFound,
                        Constants.Validation.Blogs.BlogNotFound(id));
                }

                return new BlogResponse { Blog = blog };
            }
            catch (DocumentClientException ex)
            {
                return BaseResponse.Failure<BlogResponse>(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.CosmosError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<BlogResponse>(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public BlogsResponse GetBlogs(IEnumerable<string> ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return BaseResponse.Failure<BlogsResponse>(HttpStatusCode.BadRequest,
                        Constants.Validation.Blogs.NoIdsRecieved());
                }

                var response = _blogRepository.GetBlogs(ids);

                if (response == null || !response.Any())
                {
                    return BaseResponse.Failure<BlogsResponse>(HttpStatusCode.NotFound,
                        Constants.Validation.Blogs.BlogsNotFound());
                }

                return new BlogsResponse { Blogs = response };
            }
            catch (DocumentClientException ex)
            {
                return BaseResponse.Failure<BlogsResponse>(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.CosmosError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<BlogsResponse>(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> UpdateBlogAsync(Blog blog, ModelStateDictionary modelState)
        {
            try
            {
                if (blog == null)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest,
                        Constants.Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errors = modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(err => err.ErrorMessage)
                        .Aggregate((f, s) => $"{f}; {s}");

                    return BaseResponse.Failure(HttpStatusCode.BadRequest, errors);
                }

                if (_blogRepository.CheckForExistedBlog(blog.Title))
                {
                    return BaseResponse.Failure(HttpStatusCode.Conflict,
                        Constants.Validation.Blogs.SameBlogTitleExists());
                }

                await _blogRepository.UpdateBlogAsync(blog);

                return BaseResponse.Success;
            }
            catch (DocumentClientException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.CosmosError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }
    }
}
